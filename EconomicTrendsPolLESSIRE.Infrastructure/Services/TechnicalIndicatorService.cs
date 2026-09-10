using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class TechnicalIndicatorService : ITechnicalIndicatorService
    {
        public Task<bool> DeleteTechnicalIndicatorAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TechnicalIndicator>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicatorDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicator?> GetTechnicalIndicatorByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicatorDTO?> SaveAsync(TechnicalIndicatorDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicator?> SaveTechnicalIndicatorAsync(TechnicalIndicator technicalIndic, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
