using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Infrastructure.MarketData
{
    public sealed class MarketSnapshotProjector : IMarketSnapshotProjector
    {
        private const int DefaultIntervalCode = 1;

        private readonly IMarketTradeRepository _tradeRepository;
        private readonly IMarketQuoteRepository _quoteRepository;
        private readonly IMarketCandleRepository _candleRepository;
        private readonly IMarketSnapshotRepository _snapshotRepository;
        private readonly ILogger<MarketSnapshotProjector> _logger;

        public MarketSnapshotProjector(IMarketTradeRepository tradeRepository, IMarketQuoteRepository quoteRepository, IMarketCandleRepository candleRepository, IMarketSnapshotRepository snapshotRepository, ILogger<MarketSnapshotProjector> logger)
        {
            _tradeRepository = tradeRepository;
            _quoteRepository = quoteRepository;
            _candleRepository = candleRepository;
            _snapshotRepository = snapshotRepository;
            _logger = logger;
        }

        public async Task RefreshAsync(long instrumentId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            // Important :
            // All repositories use the same scoped IDbConnection.
            // Therefore, do NOT run these queries in parallel.

            var trade = await _tradeRepository.GetLatestByInstrumentAsync(instrumentId, ct);
            var quote = await _quoteRepository.GetLatestByInstrumentAsync(instrumentId, ct);
            var candle = await _candleRepository.GetLatestByInstrumentAsync(instrumentId, DefaultIntervalCode, ct);
            var previousCandle = await _candleRepository.GetPreviousByInstrumentAsync(instrumentId, DefaultIntervalCode, ct);

            if (trade is null && quote is null && candle is null)
            {
                _logger.LogDebug(
                    "No market data available for InstrumentId={InstrumentId}.",
                    instrumentId);

                return;
            }

            var marketTimestampUtc = GetLatestTimestamp(trade?.TimestampUtc, quote?.TimestampUtc, candle?.OpenTimeUtc);
            var receivedAtUtc = GetLatestTimestamp(trade?.ReceivedAtUtc, quote?.ReceivedAtUtc);

            if (receivedAtUtc == default)
            {
                receivedAtUtc = DateTime.UtcNow;
            }

            var snapshot =
                new MarketSnapshot
                {
                    InstrumentId = instrumentId,
                    LastPrice = trade?.Price ?? candle?.ClosePrice ?? 0m,
                    BidPrice = quote?.BidPrice ?? 0m,
                    AskPrice = quote?.AskPrice ?? 0m,
                    OpenPrice = candle?.OpenPrice ?? 0m,
                    HighPrice = candle?.HighPrice ?? 0m,
                    LowPrice = candle?.LowPrice ?? 0m,
                    PreviousClose = previousCandle?.ClosePrice ?? 0m,
                    Volume = candle?.Volume ?? 0m,
                    LastProviderId = trade?.ProviderId ?? quote?.ProviderId ?? 0,
                    MarketTimestampUtc = marketTimestampUtc,
                    ReceivedAtUtc = receivedAtUtc,
                    Active = true
                };

            await _snapshotRepository.UpsertAsync(snapshot, ct);

            _logger.LogInformation(
                "Market snapshot refreshed. " +
                "InstrumentId={InstrumentId}, " +
                "LastPrice={LastPrice}, " +
                "Bid={BidPrice}, " +
                "Ask={AskPrice}, " +
                "PreviousClose={PreviousClose}",
                snapshot.InstrumentId,
                snapshot.LastPrice,
                snapshot.BidPrice,
                snapshot.AskPrice,
                snapshot.PreviousClose);
        }

        private static DateTime GetLatestTimestamp(params DateTime?[] timestamps)
        {
            return timestamps
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .DefaultIfEmpty(default)
                .Max();
        }
    }
}















































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.