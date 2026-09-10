using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        public Task<int> ArchivePastProvidersAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProviderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Provider>> GetAllProviderAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Provider?> GetProviderByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Provider?> SaveProviderAsync(Provider provider)
        {
            throw new NotImplementedException();
        }
    }
}
