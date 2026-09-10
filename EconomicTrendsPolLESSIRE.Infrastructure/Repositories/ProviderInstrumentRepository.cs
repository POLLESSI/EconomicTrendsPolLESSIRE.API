using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class ProviderInstrumentRepository : IProviderInstrumentRepository
    {
        public Task<int> ArchivePastProviderInstrumentsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProviderInstrumentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProviderInstrument>> GetAllProviderInstrumentAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrument?> GetProviderInstrumentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrument?> SaveProviderInstrumentAsync(ProviderInstrument Providinstrument)
        {
            throw new NotImplementedException();
        }
    }
}
