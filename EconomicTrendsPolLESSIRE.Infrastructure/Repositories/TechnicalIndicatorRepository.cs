using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class TechnicalIndicatorRepository : ITechnicalIndicatorRepository
    {
        public Task<int> ArchivePastTechnicalIndicatorsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTechnicalIndicatorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TechnicalIndicator>> GetAllTechnicalIndicatorAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicator?> GetTechnicalIndicatorByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicator?> SaveTechnicalIndicatorAsync(TechnicalIndicator technicalindic)
        {
            throw new NotImplementedException();
        }
    }
}
