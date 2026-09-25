using EconomicTrendsPolLESSIRE.Domain.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public sealed class MistralInteractionsRepository : IMistralInteractionRepository
    {
#nullable disable
        private readonly IDbConnection _connection;
        private readonly IConfiguration _config;
        private readonly ILogger<MistralInteractionsRepository> _logger;

        public MistralInteractionsRepository(IDbConnection connection, IConfiguration config, ILogger<MistralInteractionsRepository> logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // ======================
        // Helpers
        // ======================

        private static string NormalizePrompt(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var normalized = value.Trim().Normalize(NormalizationForm.FormKC);
            normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"\s+", " ");

            return normalized.ToLowerInvariant();
        }

        private static string HmacSha256Hex(string input, string secret)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));

            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        private string ComputePromptHash(string? prompt)
        {
            var pepper = _config["Security:PromptHashPepper"];
            if (string.IsNullOrWhiteSpace(pepper))
                throw new InvalidOperationException("Missing Security:PromptHashPepper in configuration.");

            var normalized = NormalizePrompt(prompt);
            return HmacSha256Hex(normalized, pepper);
        }

        // ======================
        // Suggestions
        // ======================

        public async Task SaveSuggestionAsync(Suggestion suggestion)
        {
            ArgumentNullException.ThrowIfNull(suggestion);

            const string sql = @"
                            INSERT INTO Suggestion
                            (
                                User_Id,
                                DateSuggestion,
                                OriginalMarketTrade,
                                SuggestedAlternatives,
                                Reason,
                                Active,
                                LocationName,
                                EventId,
                                ForecastId,
                                TrafficId
                            )
                            VALUES
                            (
                                @User_Id,
                                @DateSuggestion,
                                @OriginalMarketTrade,
                                @SuggestedAlternatives,
                                @Reason,
                                1,
                                @LocationName
                            );";

            var parameters = new DynamicParameters();
            parameters.Add("@User_Id", suggestion.User_Id, DbType.Int32);
            parameters.Add("@DateSuggestion", suggestion.DateSuggestion, DbType.DateTime2);
            parameters.Add("@OriginalMarketTrade", suggestion.OriginalPlace, DbType.String);
            parameters.Add("@SuggestedAlternatives", suggestion.SuggestedAlternatives, DbType.String);
            parameters.Add("@Reason", suggestion.Reason, DbType.String);
            parameters.Add("@LocationName", suggestion.LocationName, DbType.String);
           
            await _connection.ExecuteAsync(sql, parameters);
        }

        public async Task<IEnumerable<Suggestion>> GetAllSuggestionsAsync()
        {
            const string sql = @"SELECT * FROM Suggestion WHERE Active = 1 ORDER BY DateSuggestion DESC;";
            return await _connection.QueryAsync<Suggestion>(sql);
        }

        public async Task<IEnumerable<Suggestion>> GetSuggestionsByEventIdAsync(int id)
        {
            const string sql = @"SELECT * FROM Suggestion WHERE EventId = @Id AND Active = 1 ORDER BY DateSuggestion DESC;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);

            return await _connection.QueryAsync<Suggestion>(sql, parameters);
        }

        public async Task<IEnumerable<Suggestion>> GetSuggestionsByForecastIdAsync(int id)
        {
            const string sql = @"SELECT * FROM Suggestion WHERE ForecastId = @Id AND Active = 1 ORDER BY DateSuggestion DESC;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);

            return await _connection.QueryAsync<Suggestion>(sql, parameters);
        }

        public async Task<IEnumerable<Suggestion>> GetSuggestionsByTrafficIdAsync(int id)
        {
            const string sql = @"SELECT * FROM Suggestion WHERE TrafficId = @Id AND Active = 1 ORDER BY DateSuggestion DESC;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);

            return await _connection.QueryAsync<Suggestion>(sql, parameters);
        }

        public async Task DeleteSuggestionAsync(int id)
        {
            const string sql = @"
                            UPDATE Suggestion
                            SET Active = 0,
                                DateDeleted = SYSUTCDATETIME()
                            WHERE Id = @Id;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);

            var affected = await _connection.ExecuteAsync(sql, parameters);
            if (affected == 0)
                throw new KeyNotFoundException($"Suggestion #{id} non-existent or already deleted.");
        }

        public async Task<IEnumerable<Suggestion>> GetSuggestionsByMarketTradeCrowdAsync(string placeName)
        {
            const string sql = @"
                            SELECT s.*
                            FROM Suggestion s
                            INNER JOIN CrowdInfo c ON s.OriginalMarketTrade = c.LocationName AND c.Active = 1
                            INNER JOIN MarketTrade p ON p.Name = c.LocationName AND p.Active = 1
                            WHERE LOWER(p.Name) = LOWER(@MarketTradeName)
                              AND s.Active = 1
                            ORDER BY s.DateSuggestion DESC;";

            var parameters = new DynamicParameters();
            parameters.Add("@MarketTradeName", placeName, DbType.String);

            return await _connection.QueryAsync<Suggestion>(sql, parameters);
        }

        //public async Task<IEnumerable<SuggestionGroupedByMarketTradeDTO>> GetSuggestionsGroupedByMarketTradeAsync(
        //    string? typeFilter = null,
        //    bool? indoorFilter = null,
        //    DateTime? sinceDate = null)
        //{
        //    const string baseSql = @"
        //                        SELECT 
        //                            s.OriginalMarketTrade                            AS MarketTradeName,
        //                            MAX(p.Type)                                 AS Type,
        //                            CAST(MAX(CAST(p.Indoor AS TINYINT)) AS BIT) AS Indoor,
        //                            MAX(p.Latitude)                             AS Latitude,
        //                            MAX(p.Longitude)                            AS Longitude,
        //                            MAX(c.CrowdLevel)                           AS CrowdLevel,
        //                            COUNT(*)                                    AS SuggestionCount,
        //                            MAX(s.DateSuggestion)                       AS LastSuggestedAt
        //                        FROM Suggestion s
        //                        LEFT JOIN MarketTrade p ON s.OriginalMarketTrade = p.Name AND p.Active = 1
        //                        LEFT JOIN CrowdInfo c ON c.LocationName = s.OriginalMarketTrade AND c.Active = 1
        //                        WHERE s.Active = 1
        //                        /**WHERE_FILTER**/
        //                        GROUP BY s.OriginalMarketTrade
        //                        ORDER BY LastSuggestedAt DESC;";

        //    var filters = new List<string>();
        //    var parameters = new DynamicParameters();

        //    if (!string.IsNullOrWhiteSpace(typeFilter))
        //    {
        //        filters.Add("p.Type = @TypeFilter");
        //        parameters.Add("@TypeFilter", typeFilter, DbType.String);
        //    }

        //    if (indoorFilter.HasValue)
        //    {
        //        filters.Add("p.Indoor = @IndoorFilter");
        //        parameters.Add("@IndoorFilter", indoorFilter.Value, DbType.Boolean);
        //    }

        //    if (sinceDate.HasValue)
        //    {
        //        filters.Add("s.DateSuggestion >= @SinceDate");
        //        parameters.Add("@SinceDate", sinceDate.Value, DbType.DateTime2);
        //    }

        //    var whereClause = filters.Count > 0
        //        ? " AND " + string.Join(" AND ", filters)
        //        : string.Empty;

        //    var sql = baseSql.Replace("/**WHERE_FILTER**/", whereClause);

        //    return await _connection.QueryAsync<SuggestionGroupedByMarketTradeDTO>(sql, parameters);
        //}

        // ======================
        // Interactions Mistral
        // ======================

        public async Task SaveInteractionAsync(string prompt, string response, DateTime timestamp)
        {
            var interaction = new MistralInteraction
            {
                Prompt = prompt,
                Response = response,
                CreatedAt = timestamp,
                Active = true
            };

            await SaveInteractionAsync(interaction);
        }

        public async Task<MistralInteraction> CreatePendingAsync(
    MistralInteraction interaction,
    CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(interaction);

            interaction.Prompt ??= string.Empty;
            interaction.Response ??= string.Empty;

            interaction.Active = true;

            interaction.CreatedAt = interaction.CreatedAt == default ? DateTime.UtcNow : interaction.CreatedAt;

            interaction.Model ??= "mistral";
            interaction.Temperature ??= 0.3f;

            interaction.ExecutionSource ??= "MistralLocal";

            interaction.Status ??= "Pending";

            interaction.PromptHash = ComputePromptHash(interaction.Prompt);

            const string sql = @"
                            INSERT INTO dbo.MistralInteractions
                            (
                                Prompt,
                                PromptHash,
                                Response,
                                CreatedAt,
                                Active,

                                Model,
                                Temperature,
                                TokenCount,

                                SourceType,
                                ExecutionSource,
                                Status,

                                InstrumentId,
                                ProviderId,
                                MarketQuoteId,
                                MarketTradeId,

                                MarketCandleIntervalCode,
                                MarketCandleOpenTimeUtc,

                                TechnicalIndicatorIntervalCode,
                                TechnicalIndicatorType,
                                TechnicalIndicatorTimestampUtc,

                                Latitude,
                                Longitude
                            )
                            OUTPUT INSERTED.*
                            VALUES
                            (
                                @Prompt,
                                @PromptHash,
                                @Response,
                                SYSUTCDATETIME(),
                                1,

                                @Model,
                                @Temperature,
                                @TokenCount,

                                @SourceType,
                                @ExecutionSource,
                                @Status,

                                @InstrumentId,
                                @ProviderId,
                                @MarketQuoteId,
                                @MarketTradeId,

                                @MarketCandleIntervalCode,
                                @MarketCandleOpenTimeUtc,

                                @TechnicalIndicatorIntervalCode,
                                @TechnicalIndicatorType,
                                @TechnicalIndicatorTimestampUtc,

                                @Latitude,
                                @Longitude
                            );
                            ";

            return await _connection.QuerySingleAsync<MistralInteraction>(new CommandDefinition(sql, interaction, cancellationToken: ct));
        }

        public async Task SaveInteractionAsync(MistralInteraction interaction)
        {
            ArgumentNullException.ThrowIfNull(interaction);

            await UpsertInteractionAsync(interaction);
        }
        
        public async Task<IEnumerable<MistralInteraction>> GetAllInteractionsAsync()
        {
            const string sql = @"SELECT * FROM [MistralInteractions] WHERE Active = 1 ORDER BY CreatedAt DESC;";
            return await _connection.QueryAsync<MistralInteraction>(sql);
        }

        public async Task<MistralInteraction?> GetByIdAsync(int id)
        {
            const string sql = @"SELECT * FROM [MistralInteractions] WHERE Id = @Id AND Active = 1;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);

            return await _connection.QueryFirstOrDefaultAsync<MistralInteraction>(sql, parameters);
        }

        public async Task<string> AskAsync(string question)
        {
            var simulatedResponse = $"(Simulated Mistral Response) You asked : \"{question}\"";

            await SaveInteractionAsync(new MistralInteraction
            {
                Prompt = question,
                Response = simulatedResponse
            });

            return simulatedResponse;
        }

        public async Task<bool> DeactivateInteractionAsync(int id)
        {
            const string sql = @"
                            UPDATE dbo.MistralInteractions
                            SET
                                Active = 0,
                                DateDeleted =
                                    COALESCE(
                                        DateDeleted,
                                        SYSUTCDATETIME())
                            WHERE Id = @Id
                              AND Active = 1;
                            ";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);

            var affected = await _connection.ExecuteAsync(sql, parameters);
            return affected > 0;
        }

        public async Task<int> ArchivePastMistralInteractionsAsync()
        {
            const string sql = @"
                            UPDATE dbo.MistralInteractions
                            SET
                                Active = 0,
                                DateDeleted =
                                    COALESCE(
                                        DateDeleted,
                                        SYSUTCDATETIME())
                            WHERE Active = 1
                              AND CreatedAt <
                                  DATEADD(
                                      DAY,
                                      -1,
                                      SYSUTCDATETIME());
                            ";

            try
            {
                var affected = await _connection.ExecuteAsync(sql);
                _logger.LogInformation("{Count} Mistral interaction(s) archived.", affected);
                return affected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error archiving past Mistral interactions.");
                return 0;
            }
        }

        public async Task<MistralInteraction?> UpsertInteractionAsync(MistralInteraction interaction)
        {
            ArgumentNullException.ThrowIfNull(interaction);

            var promptHash = ComputePromptHash(interaction.Prompt);

            var parameters = new DynamicParameters();
            parameters.Add("@Prompt", interaction.Prompt, DbType.String);
            parameters.Add("@PromptHash", promptHash, DbType.String);
            parameters.Add("@Response", interaction.Response, DbType.String);
            parameters.Add("@Model", interaction.Model, DbType.String);
            parameters.Add("@Temperature", interaction.Temperature, DbType.Single);
            parameters.Add("@TokenCount", interaction.TokenCount, DbType.Int32);
            parameters.Add("@Latitude", interaction.Latitude, DbType.Double);
            parameters.Add("@Longitude", interaction.Longitude, DbType.Double);
            parameters.Add("@SourceType", interaction.SourceType, DbType.String);

            try
            {
                _logger.LogInformation("Calling sp_MistralInteraction_Upsert. PromptHash={PromptHash}, ResponseLength={ResponseLength}", promptHash, interaction.Response?.Length ?? 0);

                var result = await _connection.QuerySingleOrDefaultAsync<MistralInteraction>(
                    "dbo.sp_MistralInteraction_Upsert", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation(
                    "sp_MistralInteraction_Upsert completed. PromptHash={PromptHash}, ReturnedId={ReturnedId}, ReturnedResponseLength={ReturnedResponseLength}",
                    promptHash,
                    result?.Id ?? 0,
                    result?.Response?.Length ?? 0);

                if (result is null)
                {
                    _logger.LogError("sp_MistralInteraction_Upsert returned null for prompt: {Prompt}", interaction.Prompt);

                    throw new InvalidOperationException("Failed to upsert Mistral interaction.");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing sp_MistralInteraction_Upsert for prompt: {Prompt}", interaction.Prompt);

                throw;
            }
        }

        public async Task<bool> UpdateResponseAsync(int interactionId, string response, CancellationToken ct = default)
        {
            const string sql = @"
                            UPDATE [MistralInteractions]
                            SET Response = @Response,
                                Active = 1
                            WHERE Id = @Id;";

            var affected = await _connection.ExecuteAsync(sql, new
            {
                Id = interactionId,
                Response = response ?? string.Empty
            });

            _logger.LogInformation(
                "UpdateResponseAsync executed. InteractionId={InteractionId}, Updated={Updated}, ResponseLength={ResponseLength}",
                interactionId,
                affected > 0,
                response?.Length ?? 0);

            return affected > 0;
        }

        public async Task<bool> MarkFailedAsync(int interactionId, string? errorMessage, CancellationToken ct = default)
        {
            const string sql = @"
                            UPDATE [MistralInteractions]
                            SET Response = COALESCE(NULLIF(Response, ''), @ErrorMessage),
                                Active = 1
                            WHERE Id = @Id;";

            var safeMessage = string.IsNullOrWhiteSpace(errorMessage)
                ? "Mistral request failed."
                : $"Mistral request failed: {errorMessage}";

            var affected = await _connection.ExecuteAsync(sql, new
            {
                Id = interactionId,
                ErrorMessage = safeMessage
            });

            _logger.LogWarning(
                "MarkFailedAsync executed. InteractionId={InteractionId}, Updated={Updated}, ErrorMessage={ErrorMessage}",
                interactionId,
                affected > 0,
                safeMessage);

            return affected > 0;
        }

        public async Task<bool> MarkCancelledAsync(
    int interactionId,
    string? message = null,
    CancellationToken ct = default)
        {
            if (interactionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(interactionId));

            const string sql = """
        UPDATE dbo.MistralInteractions
        SET Response =
                CASE
                    WHEN NULLIF(LTRIM(RTRIM(Response)), '') IS NULL
                    THEN @Message
                    ELSE Response
                END,
            Active = 1
        WHERE Id = @Id;
        """;

            var safeMessage = string.IsNullOrWhiteSpace(message)
                ? "Generation cancelled."
                : message.Trim();

            var command = new CommandDefinition(
                sql,
                new
                {
                    Id = interactionId,
                    Message = safeMessage
                },
                cancellationToken: ct);

            var affected = await _connection.ExecuteAsync(command);

            _logger.LogInformation(
                "MarkCancelledAsync executed. InteractionId={InteractionId}, Updated={Updated}, Message={Message}",
                interactionId,
                affected > 0,
                safeMessage);

            return affected > 0;
        }

        public async Task<bool> UpdateLocationAsync(int interactionId, double latitude, double longitude, CancellationToken ct = default)
        {
            const string sql = """
                            UPDATE dbo.MistralInteractions
                            SET Latitude = @Latitude,
                                Longitude = @Longitude
                            WHERE Id = @InteractionId;
                            """;

            var command = new CommandDefinition(
                sql,
                new
                {
                    InteractionId = interactionId,
                    Latitude = latitude,
                    Longitude = longitude
                },
                cancellationToken: ct);

            var affectedRows = await _connection.ExecuteAsync(command);

            return affectedRows > 0;
        }

        public async Task<bool> CompleteAsync(int interactionId, string response, string sourceType, CancellationToken ct = default)
        {
            if (interactionId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(interactionId));
            }

            const string sql = """
                            UPDATE dbo.MistralInteractions
                            SET
                                Response = @Response,
                                SourceType = @SourceType,
                                Active = 1
                            WHERE Id = @InteractionId;
                            """;

            var command = new CommandDefinition(
                    sql,
                    new
                    {
                        InteractionId = interactionId,
                        Response = response ?? string.Empty,
                        SourceType = sourceType
                    },
                    cancellationToken: ct);

            var affected = await _connection.ExecuteAsync(command);

            _logger.LogInformation(
                "CompleteAsync executed. " +
                "InteractionId={InteractionId}, " +
                "SourceType={SourceType}, " +
                "ResponseLength={ResponseLength}, " +
                "Updated={Updated}",
                interactionId,
                sourceType,
                response?.Length ?? 0,
                affected > 0);

            return affected > 0;
        }
    }
}


































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.