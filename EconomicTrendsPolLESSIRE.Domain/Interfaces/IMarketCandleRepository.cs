using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IMarketCandleRepository
    {
        Task<MarketCandle?> SaveMarketCandleAsync(MarketCandle marketCdl);
        Task<IEnumerable<MarketCandle>>GetAllMarketCandleAsync(int limit = 200, CancellationToken ct = default);
        Task<MarketCandle?>GetMarketCandleByIdAsync(long instrumentId, CancellationToken ct = default);
        Task<MarketCandle?>GetLatestByInstrumentAsync(long instrumentId, int intervalCode = 1, CancellationToken ct = default);
        Task<MarketCandle?>GetPreviousByInstrumentAsync(long instrumentId, int intervalCode = 1, CancellationToken ct = default);
        Task<IReadOnlyList<MarketCandle>>GetRecentByInstrumentAsync(long instrumentId, int intervalCode, int limit, CancellationToken ct = default);
        Task<bool>DeleteMarketCandleAsync(long instrumentId);

        Task<int> ArchivePastMarketCandlesAsync(CancellationToken ct = default);

        Task UpsertBatchAsync(IReadOnlyCollection<MarketCandle> candles, CancellationToken ct = default);
    }
}





















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.