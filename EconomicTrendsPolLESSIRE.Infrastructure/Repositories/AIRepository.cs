using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
#nullable disable
    public class AIRepository : IAIRepository
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<AIRepository> _logger;

        public AIRepository(IDbConnection connection, ILogger<AIRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task SaveInteractionAsync(MistralInteraction interaction)
        {

            try
            {
                var sql = @"INSERT INTO MistralInteractions (Prompt, Response, CreatedAt, Active)
                            VALUES (@Prompt, @Response, @CreatedAt, @Active);";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Prompt", interaction.Prompt);
                parameters.Add("Response", interaction.Response);
                parameters.Add("CreatedAt", interaction.CreatedAt);
                parameters.Add("Active", interaction.Active);

                await _connection.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error saving Mistral interaction.");
                throw;
            }
        }

        public async Task<MistralInteraction?> GetByIdAsync(int id)
        {
            try
            {
                const string sql = @"SELECT * FROM MistralInteractions WHERE Id = @Id";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", id);

                return await _connection.QueryFirstOrDefaultAsync<MistralInteraction>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Mistral interaction.");
                throw;
            }

        }

        public async Task<string> GetSuggestionsAsync(object content)
        {
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true });
            return await Task.FromResult($"Suggestion (mock) based on:\n{jsonContent}");
        }

        public async Task<string> GetEconomicTrendSuggestionsAsync(string prompt)
        {
            return await Task.FromResult($"Economic Trend suggestion (mock): {prompt}");
        }

        public async Task<MistralInteraction?> GetChatMistralByIdAsync(int id)
        {
            return await GetByIdAsync(id);
        }


        public async Task<string> SummarizeTextAsync(string input)
        {
            return await Task.FromResult($"Summary (mock): {input[..Math.Min(input.Length, 80)]}...");
        }

        public async Task<string> GenerateSuggestionAsync(string prompt)
        {
            return await Task.FromResult($"Generated suggestion (mock) for : {prompt}");
        }

        public async Task<string> AskChatMistralAsync(string prompt)
        {
            return await Task.FromResult($"Mistral (mock) response to : {prompt}");
        }

        public async Task<string> TranslateToFrenchAsync(string englishText)
        {
            return await Task.FromResult($"[FR] Traduction (mock) : {englishText}");
        }

        public async Task<string> TranslateToDutchAsync(string englishText)
        {
            return await Task.FromResult($"[NL] Vertaling (mock): {englishText}");
        }

        public async Task<string> TranslateToGermanAsync(string englishText)
        {
            return await Task.FromResult($"[DE] Übersetzung (mock): {englishText}");
        }

        public async Task<string> SuggestAlternativeAsync(string prompt)
        {
            return await Task.FromResult($"Suggested alternative (mock) for : {prompt}");
        }

        public async Task<string> SuggestAlternativeWithWeatherAsync(string location)
        {
            return await Task.FromResult($"Alternative (mock) for {location} with unknown weather.");
        }
    }
}






























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.