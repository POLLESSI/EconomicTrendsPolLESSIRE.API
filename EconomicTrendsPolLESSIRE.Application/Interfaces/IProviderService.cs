using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IProviderService
    {
        Task<ProviderDTO?> GetByIdAsync(int id);
        Task<ProviderDTO?> SaveAsync(ProviderDTO dto);
        Task<Provider?> SaveProviderAsync(Provider provider, CancellationToken ct = default);
        Task<IEnumerable<Provider>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<Provider?> GetProviderByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteProviderAsync(int id, CancellationToken ct = default);
    }
}



































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.