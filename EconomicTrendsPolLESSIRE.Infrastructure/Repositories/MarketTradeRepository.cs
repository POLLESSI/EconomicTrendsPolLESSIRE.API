using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class MarketTradeRepository : IMarketTradeRepository
    {
        public Task<int> ArchivePastMarketTradesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMarketTradeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarketTrade>> GetAllMarketTradeAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketTrade?> GetMarketTradeByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MarketTrade?> SaveMarketTradeAsync(MarketTrade markettrd)
        {
            throw new NotImplementedException();
        }
    }
}
