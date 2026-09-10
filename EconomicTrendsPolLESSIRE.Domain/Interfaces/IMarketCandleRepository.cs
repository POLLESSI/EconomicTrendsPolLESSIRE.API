using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IMarketCandleRepository
    {
        Task<MarketCandle?> SaveMarketCandleAsync(MarketCandle marketCdl);
        Task<IEnumerable<MarketCandle>> GetAllMarketCandleAsync(int limit = 200, CancellationToken ct = default);
        Task<MarketCandle?> GetMarketCandleByIdAsync(int id);
        Task<bool> DeleteMarketCandleAsync(int id);
        Task<int> ArchivePastMarketCandlesAsync(CancellationToken ct = default);
    }
}
