using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IProviderRepository
    {
        Task<Provider?> SaveProviderAsync(Provider provider);
        Task<IEnumerable<Provider>> GetAllProviderAsync(int limit = 200, CancellationToken ct = default);
        Task<Provider?> GetProviderByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteProviderAsync(int id);
        Task<int> ArchivePastProvidersAsync(CancellationToken ct = default);
    }
}























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.