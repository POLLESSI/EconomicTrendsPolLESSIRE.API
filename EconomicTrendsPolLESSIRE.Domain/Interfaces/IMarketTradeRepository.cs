using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IMarketTradeRepository
    {
        Task<MarketTrade?> SaveMarketTradeAsync(MarketTrade markettrd);
        Task<IEnumerable<MarketTrade>> GetAllMarketTradeAsync(int limit = 200, CancellationToken ct = default);
        Task<MarketTrade?> GetMarketTradeByIdAsync(long id, CancellationToken ct = default);
        Task<bool> DeleteMarketTradeAsync(long id);
        Task<int> ArchivePastMarketTradesAsync(CancellationToken ct = default);
    }
}
































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.