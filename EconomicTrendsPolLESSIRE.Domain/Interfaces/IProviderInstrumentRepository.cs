using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IProviderInstrumentRepository
    {
        Task<ProviderInstrument?> SaveProviderInstrumentAsync(ProviderInstrument providinstrument);
        Task<IEnumerable<ProviderInstrument>> GetAllProviderInstrumentAsync(int limit = 200, CancellationToken ct = default);
        Task<ProviderInstrument?> GetProviderInstrumentByIdAsync(int providerId, long instrumentId, CancellationToken ct = default);
        Task<bool> DeleteProviderInstrumentAsync(int providerId, long instrumentId);
        Task<int> ArchivePastProviderInstrumentsAsync(CancellationToken ct = default);
    }
}


























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.