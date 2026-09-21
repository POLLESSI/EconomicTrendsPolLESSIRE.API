using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IMarketQuoteRepository
    {
        Task<MarketQuote?> SaveMarketQuoteAsync(MarketQuote marketQt);
        Task<IEnumerable<MarketQuote>> GetAllMarketQuoteAsync(int limit = 200, CancellationToken ct = default);
        Task<MarketQuote?> GetMarketQuoteByIdAsync(long id, CancellationToken ct = default);
        Task<bool> DeleteMarketQuoteAsync(long id);
        Task<int> ArchivePastMarketQuotesAsync(CancellationToken ct = default);
    }
}


























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.