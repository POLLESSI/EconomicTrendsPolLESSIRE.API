using Dapper;
using EconomicTrendsPolLESSIRE.Domain.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.RegularExpressions;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public sealed class LocalAiDataRepository : ILocalAiDataRepository
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<LocalAiDataRepository> _logger;

        public LocalAiDataRepository(IDbConnection connection, ILogger<LocalAiDataRepository> logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // =====================================================
        // INSTRUMENT SEARCH
        // =====================================================

        public async Task<IReadOnlyList<LocalAiInstrumentContextDTO>> SearchInstrumentsByKeywordsAsync(string userPrompt, int limit = 10, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(userPrompt))
            {
                return Array.Empty<LocalAiInstrumentContextDTO>();
            }

            var tokens = ExtractSearchTokens(userPrompt);

            if (tokens.Count == 0)
            {
                return Array.Empty<LocalAiInstrumentContextDTO>();
            }

            var effectiveLimit = Math.Clamp(limit, 1, 50);

            /*
             * For the first implementation,
             * we search the complete prompt and its tokens.
             *
             * Symbol exact matches are preferred.
             */
            const string sql = @"
                            SELECT TOP (@Limit)
                                I.Id,
                                I.Symbol,
                                I.Name,
                                I.AssetClass,
                                I.ExchangeCode,
                                I.CurrencyCode,
                                I.CreatedAtUtc,
                                I.Active
                            FROM dbo.Instrument AS I
                            WHERE I.Active = 1
                              AND
                              (
                                  I.Symbol LIKE @Search
                                  OR I.Name LIKE @Search
                                  OR I.ExchangeCode LIKE @Search
                                  OR I.CurrencyCode LIKE @Search
                              )
                            ORDER BY
                                CASE
                                    WHEN I.Symbol = @Exact THEN 0
                                    WHEN I.Symbol LIKE @Prefix THEN 1
                                    WHEN I.Name LIKE @Search THEN 2
                                    ELSE 3
                                END,
                                I.Symbol,
                                I.Name;
                            ";

            var results = new Dictionary<long, LocalAiInstrumentContextDTO>();

            foreach (var token in tokens)
            {
                ct.ThrowIfCancellationRequested();

                var parameters = new DynamicParameters();

                parameters.Add("@Limit", effectiveLimit, DbType.Int32);
                parameters.Add("@Exact", token, DbType.String);
                parameters.Add("@Prefix", $"{token}%", DbType.String);
                parameters.Add("@Search", $"%{token}%", DbType.String);

                var command = new CommandDefinition(sql, parameters, cancellationToken: ct);

                var rows = await _connection.QueryAsync<LocalAiInstrumentContextDTO>(command);

                foreach (var row in rows)
                {
                    results.TryAdd(row.Id, row);
                }

                if (results.Count >= effectiveLimit)
                {
                    break;
                }
            }

            return results.Values
                .Take(effectiveLimit)
                .ToList();
        }

        // =====================================================
        // INSTRUMENTS
        // =====================================================

        public async Task<IReadOnlyList<LocalAiInstrumentContextDTO>> GetActiveInstrumentsAsync( int limit = 100, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP (@Limit)
                                Id,
                                Symbol,
                                Name,
                                AssetClass,
                                ExchangeCode,
                                CurrencyCode,
                                CreatedAtUtc,
                                Active
                            FROM dbo.Instrument
                            WHERE Active = 1
                            ORDER BY Symbol, Name;
                            ";

            return (
                await _connection.QueryAsync<
                    LocalAiInstrumentContextDTO>(
                    new CommandDefinition(
                        sql,
                        new
                        {
                            Limit = Math.Clamp(limit, 1, 500)
                        },
                        cancellationToken: ct)))
                .AsList();
        }

        // =====================================================
        // MARKET CANDLES
        // =====================================================

        public async Task<IReadOnlyList<LocalAiMarketCandleContextDTO>> GetMarketCandlesAsync(DateTime dateFrom, DateTime dateToExclusive, int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP (@Limit)
                                *
                            FROM dbo.MarketCandle
                            WHERE OpenTimeUtc >= @DateFrom
                              AND OpenTimeUtc < @DateToExclusive
                              AND Active = 1
                            ORDER BY OpenTimeUtc DESC;
                            ";

            return (
                await _connection.QueryAsync<
                    LocalAiMarketCandleContextDTO>(
                    new CommandDefinition(
                        sql,
                        new
                        {
                            DateFrom = dateFrom,
                            DateToExclusive =
                                dateToExclusive,
                            Limit =
                                Math.Clamp(
                                    limit,
                                    1,
                                    1000)
                        },
                        cancellationToken: ct)))
                .AsList();
        }

        // =====================================================
        // MARKET QUOTES
        // =====================================================

        public async Task<IReadOnlyList<LocalAiMarketQuoteContextDTO>> GetMarketQuotesAsync(DateTime dateFrom, DateTime dateToExclusive, int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP (@Limit)
                                *
                            FROM dbo.MarketQuote
                            WHERE TimestampUtc >= @DateFrom
                              AND TimestampUtc < @DateToExclusive
                              AND Active = 1
                            ORDER BY TimestampUtc DESC;
                            ";

            return (
                await _connection.QueryAsync<
                    LocalAiMarketQuoteContextDTO>(
                    new CommandDefinition(
                        sql,
                        new
                        {
                            DateFrom = dateFrom,
                            DateToExclusive = dateToExclusive,
                            Limit = Math.Clamp(limit, 1, 1000)
                        },
                        cancellationToken: ct)))
                .AsList();
        }

        // =====================================================
        // CURRENT MARKET SNAPSHOTS
        // =====================================================

        public async Task<IReadOnlyList<LocalAiMarketSnapshotContextDTO>> GetMarketSnapshotsAsync(int limit = 100, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP (@Limit)
                                *
                            FROM dbo.MarketSnapshot
                            WHERE Active = 1
                            ORDER BY
                                MarketTimestampUtc DESC,
                                ReceivedAtUtc DESC;
                            ";

            return (
                await _connection.QueryAsync<
                    LocalAiMarketSnapshotContextDTO>(
                    new CommandDefinition(
                        sql,
                        new
                        {
                            Limit = Math.Clamp(limit, 1, 500)
                        },
                        cancellationToken: ct)))
                .AsList();
        }

        // =====================================================
        // MARKET TRADES
        // =====================================================

        public async Task<IReadOnlyList<LocalAiMarketTradeContextDTO>> GetMarketTradesAsync(DateTime dateFrom, DateTime dateToExclusive, int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP (@Limit)
                                *
                            FROM dbo.MarketTrade
                            WHERE TimestampUtc >= @DateFrom
                              AND TimestampUtc < @DateToExclusive
                              AND Active = 1
                            ORDER BY TimestampUtc DESC;
                            ";

            return (
                await _connection.QueryAsync<
                    LocalAiMarketTradeContextDTO>(
                    new CommandDefinition(
                        sql,
                        new
                        {
                            DateFrom = dateFrom,
                            DateToExclusive = dateToExclusive,
                            Limit = Math.Clamp(limit, 1, 1000)
                        },
                        cancellationToken: ct)))
                .AsList();
        }

        // =====================================================
        // PROVIDERS
        // =====================================================

        public async Task<IReadOnlyList<LocalAiProviderContextDTO>> GetActiveProvidersAsync(int limit = 50, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP (@Limit)
                                *
                            FROM dbo.Provider
                            WHERE Active = 1
                            ORDER BY Code, Name;
                            ";

            return (
                await _connection.QueryAsync<
                    LocalAiProviderContextDTO>(
                    new CommandDefinition(
                        sql,
                        new
                        {
                            Limit = Math.Clamp(limit, 1, 100)
                        },
                        cancellationToken: ct)))
                .AsList();
        }

        // =====================================================
        // PROVIDER INSTRUMENTS
        // =====================================================

        public async Task<IReadOnlyList<LocalAiProviderInstrumentContextDTO>> GetActiveProviderInstrumentsAsync(int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP (@Limit)
                                *
                            FROM dbo.ProviderInstrument
                            WHERE Active = 1
                            ORDER BY ProviderId, InstrumentId;
                            ";

            return (
                await _connection.QueryAsync<
                    LocalAiProviderInstrumentContextDTO>(
                    new CommandDefinition(
                        sql,
                        new
                        {
                            Limit = Math.Clamp(limit, 1, 1000)
                        },
                        cancellationToken: ct)))
                .AsList();
        }

        // =====================================================
        // TECHNICAL INDICATORS
        // =====================================================

        public async Task<IReadOnlyList<LocalAiTechnicalIndicatorContextDTO>> GetTechnicalIndicatorsAsync(DateTime dateFrom, DateTime dateToExclusive, int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP (@Limit)
                                *
                            FROM dbo.TechnicalIndicator
                            WHERE TimestampUtc >= @DateFrom
                              AND TimestampUtc < @DateToExclusive
                              AND Active = 1
                            ORDER BY TimestampUtc DESC;
                            ";

            return (
                await _connection.QueryAsync<
                    LocalAiTechnicalIndicatorContextDTO>(
                    new CommandDefinition(
                        sql,
                        new
                        {
                            DateFrom = dateFrom,
                            DateToExclusive = dateToExclusive,
                            Limit = Math.Clamp(limit, 1, 1000)
                        },
                        cancellationToken: ct)))
                .AsList();
        }

        // =====================================================
        // RECENT USER MESSAGES
        // =====================================================

        public async Task<IReadOnlyList<LocalAiUserMessageContextDTO>> GetRecentUserMessagesAsync(DateTime sinceUtc, int limit = 10, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP (@Limit)
                                *
                            FROM dbo.UserMessage
                            WHERE Active = 1
                              AND CreatedAt >= @SinceUtc
                              AND NULLIF(
                                    LTRIM(RTRIM(Content)),
                                    '') IS NOT NULL
                            ORDER BY CreatedAt DESC;
                            ";

            return (
                await _connection.QueryAsync<
                    LocalAiUserMessageContextDTO>(
                    new CommandDefinition(
                        sql,
                        new
                        {
                            SinceUtc = sinceUtc,
                            Limit = Math.Clamp(limit, 1, 50)
                        },
                        cancellationToken: ct)))
                .AsList();
        }

        // =====================================================
        // OPTIONAL GEOGRAPHIC USER MESSAGES
        // =====================================================

        public async Task<IReadOnlyList<LocalAiUserMessageContextDTO>> GetNearbyUserMessagesAsync(double latitude, double longitude, double radiusKm, DateTime sinceUtc, int limit = 10, CancellationToken ct = default)
        {
            const string sql = @"
                                DECLARE @Origin geography =
                                    geography::Point(
                                        @Latitude,
                                        @Longitude,
                                        4326);

                                WITH CandidateMessages AS
                                (
                                    SELECT
                                        UM.*,

                                        DistanceKm =
                                            @Origin.STDistance(
                                                geography::Point(
                                                    CAST(UM.Latitude AS float),
                                                    CAST(UM.Longitude AS float),
                                                    4326))
                                            / 1000.0

                                    FROM dbo.UserMessage AS UM

                                    WHERE UM.Active = 1

                                      AND UM.CreatedAt >= @SinceUtc

                                      AND UM.Latitude IS NOT NULL
                                      AND UM.Longitude IS NOT NULL

                                      AND UM.Latitude
                                          BETWEEN -90 AND 90

                                      AND UM.Longitude
                                          BETWEEN -180 AND 180

                                      AND NOT
                                      (
                                          UM.Latitude = 0
                                          AND UM.Longitude = 0
                                      )

                                      AND NULLIF(
                                            LTRIM(RTRIM(UM.Content)),
                                            '') IS NOT NULL
                                )

                                SELECT TOP (@Limit)
                                    *
                                FROM CandidateMessages
                                WHERE DistanceKm <= @RadiusKm
                                ORDER BY
                                    CreatedAt DESC,
                                    DistanceKm ASC;
                                ";

            var parameters =
                new DynamicParameters();

            parameters.Add(
                "@Latitude",
                latitude,
                DbType.Double);

            parameters.Add(
                "@Longitude",
                longitude,
                DbType.Double);

            parameters.Add(
                "@RadiusKm",
                radiusKm,
                DbType.Double);

            parameters.Add(
                "@SinceUtc",
                sinceUtc,
                DbType.DateTime2);

            parameters.Add(
                "@Limit",
                Math.Clamp(
                    limit,
                    1,
                    50),
                DbType.Int32);

            return (
                await _connection.QueryAsync<
                    LocalAiUserMessageContextDTO>(
                    new CommandDefinition(
                        sql,
                        parameters,
                        cancellationToken: ct)))
                .AsList();
        }

        // =====================================================
        // SEARCH TOKENIZATION
        // =====================================================

        private static IReadOnlyList<string> ExtractSearchTokens(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                return Array.Empty<string>();
            }

            var stopWords =
                new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "the",
                    "and",
                    "for",
                    "avec",
                    "dans",
                    "des",
                    "les",
                    "une",
                    "sur",
                    "pour",
                    "quel",
                    "quelle",
                    "quels",
                    "quelles",
                    "tendance",
                    "tendances",
                    "marché",
                    "marche",
                    "market",
                    "trend",
                    "analyse",
                    "analyser"
                };

            return Regex
                .Matches(prompt, @"[\p{L}\p{N}._/\-]{2,}")
                .Select(match => match.Value.Trim())
                .Where(token => !stopWords.Contains(token))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(12)
                .ToList();
        }
    }
}

































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.