using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class ProviderService : IProviderService
    {
        public Task<bool> DeleteProviderAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Provider>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Provider?> GetProviderByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderDTO?> SaveAsync(ProviderDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<Provider?> SaveProviderAsync(Provider provider, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
