using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IMarketQuoteRepository
    {
        Task<MarketQuote?> SaveMarketQuoteAsync(MarketQuote marketQt);
        Task<IEnumerable<MarketQuote>> GetAllMarketQuoteAsync(int limit = 200, CancellationToken ct = default);
        Task<MarketQuote?> GetMarketQuoteByIdAsync(int id);
        Task<bool> DeleteMarketQuoteAsync(int id);
        Task<int> ArchivePastMarketQuotesAsync(CancellationToken ct = default);
    }
}
