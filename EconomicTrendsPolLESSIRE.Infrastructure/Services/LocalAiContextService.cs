using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Domain.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public sealed class LocalAiContextService : ILocalAiContextService
    {
        private const double DefaultRadiusKm = 25d;
        private const int KeywordInstrumentLimit = 10;
        private const int UserMessageLimit = 10;
        private readonly ILocalAiDataRepository _repository;
        private readonly ILogger<LocalAiContextService> _logger;
        private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = false };

        public LocalAiContextService(ILocalAiDataRepository repository, ILogger<LocalAiContextService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<LocalAiContextDTO> BuildContextAsync(string prompt, double? latitude, double? longitude, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var safePrompt = prompt?.Trim() ?? string.Empty;

            /*
             * First version of EconomicTrends :
             *
             * We use the current UTC day.
             *
             * We can then add a real
             * temporal parser if the prompt contains :
             *
             * "Today"
             * "yesterday"
             * "this week"
             * "over the last 30 days"
             * etc.
             */
            var dateFrom = DateTime.UtcNow.Date;
            var dateToExclusive = dateFrom.AddDays(1);
            var radiusKm = DefaultRadiusKm;

            // -------------------------------------------------
            // Position-independent text search
            // -------------------------------------------------

            Task<IReadOnlyList<LocalAiInstrumentContextDTO>> keywordTask = _repository.SearchInstrumentsByKeywordsAsync(safePrompt, KeywordInstrumentLimit, ct);

            /*
             * Current methods of
             * ILocalAiDataRepository still requires
             * latitude / longitude / radiusKm.
             *
             * If no position is available,
             * We absolutely do not let them get away with it 0 / 0.
             */
            if (!HasValidCoordinates(latitude, longitude))
            {
                var keywordInstruments = await keywordTask.ConfigureAwait(false);

                var noGeoContext = new LocalAiContextDTO
                {
                    UserPrompt = safePrompt,
                    Latitude = latitude,
                    Longitude = longitude,
                    RadiusKm = radiusKm,
                    DateFrom = dateFrom,
                    DateToExclusive = dateToExclusive,
                    KeywordMatchedInstruments = keywordInstruments,
                    Instruments = keywordInstruments
                };

                LogContext(noGeoContext);

                return noGeoContext;
            }

            var lat = latitude!.Value;
            var lng = longitude!.Value;

            // -------------------------------------------------
            // Parallel EconomicTrends context acquisition
            // -------------------------------------------------

            Task<IEnumerable<LocalAiInstrumentContextDTO>>instrumentsTask = _repository.GetNearbyInstrumentsAsync(lat, lng, radiusKm, ct);
            Task<IEnumerable<LocalAiMarketCandleContextDTO>>candlesTask = _repository.GetNearbyMarketCandlesAsync(lat, lng, dateFrom, dateToExclusive, radiusKm, ct);
            Task<IEnumerable<LocalAiMarketQuoteContextDTO>>quotesTask = _repository.GetNearbyMarketQuoteAsync(lat, lng, dateFrom, dateToExclusive, radiusKm, ct);
            Task<IEnumerable<LocalAiMarketSnapshotContextDTO>>snapshotsTask = _repository.GetNearbyMarketSnapshotAsync(lat, lng, dateFrom, radiusKm, ct);
            Task<IEnumerable<LocalAiMarketTradeContextDTO>>tradesTask = _repository.GetNearbyMarketTradeContextAsync(lat, lng, dateFrom, dateToExclusive, radiusKm, ct);
            Task<IEnumerable<LocalAiProviderContextDTO>>providersTask = _repository.GetNearbyProviderAsync(lat, lng, dateFrom, dateToExclusive, radiusKm, ct);
            Task<IEnumerable<LocalAiProviderInstrumentContextDTO>>providerInstrumentsTask = _repository.GetNearbyProviderInstrumentsAsync(lat, lng, radiusKm, ct);
            Task<IEnumerable<LocalAiTechnicalIndicatorContextDTO>>indicatorsTask = _repository.GetNearbyTechnicalIndicatorsAsync(lat, lng, radiusKm, ct);
            Task<IEnumerable<LocalAiUserMessageContextDTO>>userMessagesTask = _repository.GetNearbyUserMessagesAsync(lat, lng, radiusKm, sinceUtc: DateTime.UtcNow.AddDays(-1), limit: UserMessageLimit, ct: ct);

            await Task.WhenAll(keywordTask, instrumentsTask, candlesTask, quotesTask, snapshotsTask, tradesTask, providersTask, providerInstrumentsTask, indicatorsTask, userMessagesTask).ConfigureAwait(false);

            // -------------------------------------------------
            // Materialization
            // -------------------------------------------------

            var keywordInstrumentsResult = await keywordTask.ConfigureAwait(false);
            var instruments = (await instrumentsTask.ConfigureAwait(false)).ToList();
            var candles = (await candlesTask.ConfigureAwait(false)).ToList();
            var quotes = (await quotesTask.ConfigureAwait(false)).ToList();
            var snapshots = (await snapshotsTask.ConfigureAwait(false)).ToList();
            var trades = (await tradesTask.ConfigureAwait(false)).ToList();
            var providers = (await providersTask.ConfigureAwait(false)).ToList();
            var providerInstruments = (await providerInstrumentsTask.ConfigureAwait(false)).ToList();
            var indicators = (await indicatorsTask.ConfigureAwait(false)).ToList();
            var userMessages = (await userMessagesTask.ConfigureAwait(false)).ToList();
            var context =
                new LocalAiContextDTO
                {
                    UserPrompt = safePrompt,
                    Latitude = lat,
                    Longitude = lng,
                    RadiusKm = radiusKm,
                    DateFrom = dateFrom,
                    DateToExclusive = dateToExclusive,
                    KeywordMatchedInstruments = keywordInstrumentsResult,
                    Instruments = instruments,
                    MarketCandles = candles,
                    MarketQuotes = quotes,
                    MarketSnapshots = snapshots,
                    MarketTrades = trades,
                    Providers = providers,
                    ProviderInstruments = providerInstruments,
                    TechnicalIndicators = indicators,
                    UserMessages = userMessages
                };

            LogContext(context);

            return context;
        }

        public string BuildPrompt(LocalAiContextDTO context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var sb = new StringBuilder(8192);

            sb.AppendLine("You are the EconomicTrendsPolLESSIRE local AI assistant.");
            sb.AppendLine("Analyse only the verified economic and market data supplied below.");
            sb.AppendLine("Never invent an instrument, quote, trade, price, indicator, provider or market fact.");
            sb.AppendLine("If the supplied data is insufficient, say so explicitly.");
            sb.AppendLine();
            sb.AppendLine("USER REQUEST:");
            sb.AppendLine(context.UserPrompt);
            sb.AppendLine();
            sb.AppendLine("QUERY CONTEXT:");
            sb.AppendLine($"DateFrom={context.DateFrom:O}");
            sb.AppendLine($"DateToExclusive={context.DateToExclusive:O}");

            if (context.HasCoordinates)
            {
                sb.AppendLine($"Latitude={context.Latitude}");
                sb.AppendLine($"Longitude={context.Longitude}");
                sb.AppendLine($"RadiusKm={context.RadiusKm:0.##}");
            }

            AppendJsonSection(sb, "KEYWORD MATCHED INSTRUMENTS", context.KeywordMatchedInstruments);
            AppendJsonSection(sb, "INSTRUMENTS", context.Instruments);
            AppendJsonSection(sb, "MARKET CANDLES", context.MarketCandles);
            AppendJsonSection(sb, "MARKET QUOTES", context.MarketQuotes);
            AppendJsonSection(sb, "MARKET SNAPSHOTS", context.MarketSnapshots);
            AppendJsonSection(sb, "MARKET TRADES", context.MarketTrades);
            AppendJsonSection(sb, "PROVIDERS", context.Providers);
            AppendJsonSection(sb, "PROVIDER INSTRUMENTS", context.ProviderInstruments);
            AppendJsonSection(sb, "TECHNICAL INDICATORS", context.TechnicalIndicators);
            AppendJsonSection(sb, "USER MESSAGES", context.UserMessages);

            sb.AppendLine();
            sb.AppendLine("ANALYSIS RULES:");

            sb.AppendLine("- Base every conclusion on the supplied context.");
            sb.AppendLine("- Distinguish observed market data from interpretation.");
            sb.AppendLine("- Do not invent missing prices, volumes, timestamps or indicators.");
            sb.AppendLine("- Mention uncertainty when the context is incomplete.");
            sb.AppendLine("- Prefer the most recent supplied data when timestamps differ.");

            return sb.ToString().Trim();
        }

        private static void AppendJsonSection<T>(StringBuilder sb, string sectionName, IReadOnlyList<T>? values)
        {
            sb.AppendLine();
            sb.AppendLine($"{sectionName}:");

            if (values is null || values.Count == 0)
            {
                sb.AppendLine("[]");
                return;
            }

            sb.AppendLine(JsonSerializer.Serialize(values, JsonOptions));
        }

        private void LogContext(LocalAiContextDTO context)
        {
            _logger.LogInformation(
                "[LOCAL-AI] Context built. " +
                "Instruments={Instruments}; " +
                "KeywordInstruments={KeywordInstruments}; " +
                "Candles={Candles}; " +
                "Quotes={Quotes}; " +
                "Snapshots={Snapshots}; " +
                "Trades={Trades}; " +
                "Providers={Providers}; " +
                "ProviderInstruments={ProviderInstruments}; " +
                "Indicators={Indicators}; " +
                "UserMessages={UserMessages}",
                context.Instruments.Count,
                context.KeywordMatchedInstruments.Count,
                context.MarketCandles.Count,
                context.MarketQuotes.Count,
                context.MarketSnapshots.Count,
                context.MarketTrades.Count,
                context.Providers.Count,
                context.ProviderInstruments.Count,
                context.TechnicalIndicators.Count,
                context.UserMessages.Count);
        }

        private static bool HasValidCoordinates(double? latitude, double? longitude)
        {
            return
                latitude is >= -90d and <= 90d &&
                longitude is >= -180d and <= 180d;
        }
    }
}






























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.