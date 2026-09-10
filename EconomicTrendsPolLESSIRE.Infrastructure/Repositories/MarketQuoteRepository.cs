using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class MarketQuoteRepository : IMarketQuoteRepository
    {
        public Task<int> ArchivePastMarketQuotesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMarketQuoteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarketQuote>> GetAllMarketQuoteAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketQuote?> GetMarketQuoteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MarketQuote?> SaveMarketQuoteAsync(MarketQuote marketQt)
        {
            throw new NotImplementedException();
        }
    }
}
