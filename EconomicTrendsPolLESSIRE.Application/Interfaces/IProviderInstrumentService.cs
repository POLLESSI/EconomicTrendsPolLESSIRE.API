using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IProviderInstrumentService
    {
        Task<ProviderInstrumentDTO?> GetByIdAsync(int id);
        Task<ProviderInstrumentDTO?> SaveAsync(ProviderInstrumentDTO dto);
        Task<ProviderInstrument?> SaveProviderInstrumentAsync(ProviderInstrument providerinstrum, CancellationToken ct = default);
        Task<IEnumerable<ProviderInstrument>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<ProviderInstrument?> GetProviderInstrumentByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteProviderInstrumentAsync(int id, CancellationToken ct = default);
    }
}
