using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Application.MarketData;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public sealed class MarketIngestionPipeline : IMarketIngestionPipeline
    {
        private readonly IMarketDataSource _source;
        private readonly IProviderInstrumentRepository _providerInstrumentRepository;
        private readonly IMarketTradeRepository _tradeRepository;
        private readonly IMarketQuoteRepository _quoteRepository;
        private readonly IMarketCandleRepository _candleRepository;
        private readonly IMarketSnapshotProjector _snapshotProjector;
        private readonly ITechnicalIndicatorCalculator _indicatorCalculator;
        private readonly IMarketRealtimePublisher _publisher;
        private readonly ILogger<MarketIngestionPipeline> _logger;

        public MarketIngestionPipeline(IMarketDataSource source, IProviderInstrumentRepository providerInstrumentRepository, IMarketTradeRepository tradeRepository, IMarketQuoteRepository quoteRepository, IMarketCandleRepository candleRepository, IMarketSnapshotProjector snapshotProjector, ITechnicalIndicatorCalculator indicatorCalculator, IMarketRealtimePublisher publisher, ILogger<MarketIngestionPipeline> logger)
        {
            _source = source;
            _providerInstrumentRepository = providerInstrumentRepository;

            _tradeRepository = tradeRepository;
            _quoteRepository = quoteRepository;
            _candleRepository = candleRepository;

            _snapshotProjector = snapshotProjector;
            _indicatorCalculator = indicatorCalculator;

            _publisher = publisher;
            _logger = logger;
        }

        public async Task RunOnceAsync(CancellationToken ct = default)
        {
            var mappings = await _providerInstrumentRepository.GetActiveSubscriptionsAsync(_source.Code, ct);

            if (mappings.Count == 0)
            {
                _logger.LogDebug("No active subscriptions for provider {Provider}.", _source.Code);

                return;
            }

            var subscriptions =
                mappings
                    .Select(x =>
                        new ProviderSubscription(
                            x.InstrumentId,
                            x.ProviderId,
                            x.ProviderSymbol))
                    .ToArray();

            var batch =
                await _source.GetMarketDataAsync(
                    subscriptions,
                    ct);

            var trades =
                batch.Trades
                    .Select(x =>
                        new MarketTrade
                        {
                            InstrumentId = x.InstrumentId,
                            ProviderId = x.ProviderId,
                            TimestampUtc = x.TimestampUtc,
                            ReceivedAtUtc = x.ReceivedAtUtc,
                            Price = x.Price,
                            Quantity = x.Quantity,
                            SequenceNumber = x.SequenceNumber
                        })
                    .ToArray();

            var quotes =
                batch.Quotes
                    .Select(x =>
                        new MarketQuote
                        {
                            InstrumentId = x.InstrumentId,
                            ProviderId = x.ProviderId,
                            TimestampUtc = x.TimestampUtc,
                            ReceivedAtUtc = x.ReceivedAtUtc,
                            BidPrice = x.BidPrice,
                            BidSize = x.BidSize,
                            AskPrice = x.AskPrice,
                            AskSize = x.AskSize
                        })
                    .ToArray();

            var candles =
                batch.Candles
                    .Select(x =>
                        new MarketCandle
                        {
                            InstrumentId = x.InstrumentId,
                            IntervalCode = x.IntervalCode,
                            OpenTimeUtc = x.OpenTimeUtc,
                            OpenPrice = x.OpenPrice,
                            HighPrice = x.HighPrice,
                            LowPrice = x.LowPrice,
                            ClosePrice = x.ClosePrice,
                            Volume = x.Volume,
                            VWAP = x.VWAP,
                            TradeCount = x.TradeCount,
                            IsFinal = x.IsFinal
                        })
                    .ToArray();

            await _tradeRepository
                .InsertBatchAsync(
                    trades,
                    ct);

            await _quoteRepository
                .InsertBatchAsync(
                    quotes,
                    ct);

            await _candleRepository
                .UpsertBatchAsync(
                    candles,
                    ct);

            var instrumentIds =
                subscriptions
                    .Select(x => x.InstrumentId)
                    .Distinct()
                    .ToArray();

            foreach (var instrumentId in instrumentIds)
            {
                await _snapshotProjector
                    .RefreshAsync(
                        instrumentId,
                        ct);

                await _indicatorCalculator
                    .RefreshAsync(
                        instrumentId,
                        ct);
            }

            await _publisher.PublishAsync(
                MarketHubMethods.ToClient.MarketDataRefreshed,
                instrumentIds,
                ct);

            _logger.LogInformation(
                "Market ingestion completed. " +
                "Trades={TradeCount}, " +
                "Quotes={QuoteCount}, " +
                "Candles={CandleCount}",
                trades.Length,
                quotes.Length,
                candles.Length);
        }
    }
}



























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.