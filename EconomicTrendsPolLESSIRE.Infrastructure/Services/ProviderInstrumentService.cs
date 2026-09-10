using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class ProviderInstrumentService : IProviderInstrumentService
    {
        public Task<bool> DeleteProviderInstrumentAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProviderInstrument>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrumentDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrument?> GetProviderInstrumentByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrumentDTO?> SaveAsync(ProviderInstrumentDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrument?> SaveProviderInstrumentAsync(ProviderInstrument providerinstrum, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
