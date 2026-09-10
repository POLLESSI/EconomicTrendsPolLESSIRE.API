using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class MarketCandleRepository : IMarketCandleRepository
    {
        public Task<int> ArchivePastMarketCandlesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMarketCandleAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarketCandle>> GetAllMarketCandleAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketCandle?> GetMarketCandleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MarketCandle?> SaveMarketCandleAsync(MarketCandle marketCdl)
        {
            throw new NotImplementedException();
        }
    }
}
