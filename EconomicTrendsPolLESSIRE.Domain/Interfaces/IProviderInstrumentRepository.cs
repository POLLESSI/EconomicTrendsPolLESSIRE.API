using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IProviderInstrumentRepository
    {
        Task<ProviderInstrument?> SaveProviderInstrumentAsync(ProviderInstrument Providinstrument);
        Task<IEnumerable<ProviderInstrument>> GetAllProviderInstrumentAsync(int limit = 200, CancellationToken ct = default);
        Task<ProviderInstrument?> GetProviderInstrumentByIdAsync(int id);
        Task<bool> DeleteProviderInstrumentAsync(int id);
        Task<int> ArchivePastProviderInstrumentsAsync(CancellationToken ct = default);
    }
}
