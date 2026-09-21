using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IMarketCandleRepository
    {
        Task<MarketCandle?> SaveMarketCandleAsync(MarketCandle marketCdl);
        Task<IEnumerable<MarketCandle>> GetAllMarketCandleAsync(int limit = 200, CancellationToken ct = default);
        Task<MarketCandle?> GetMarketCandleByIdAsync(long instrumentId, CancellationToken ct = default);
        Task<bool> DeleteMarketCandleAsync(long instrumentId);
        Task<int> ArchivePastMarketCandlesAsync(CancellationToken ct = default);
    }
}





















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.