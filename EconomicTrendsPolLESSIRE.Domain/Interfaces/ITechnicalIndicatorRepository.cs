using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface ITechnicalIndicatorRepository
    {
        Task<TechnicalIndicator?> SaveTechnicalIndicatorAsync(TechnicalIndicator technicalindic);
        Task<IEnumerable<TechnicalIndicator>> GetAllTechnicalIndicatorAsync(int limit = 200, CancellationToken ct = default);
        Task<TechnicalIndicator?> GetTechnicalIndicatorByIdAsync(int id);
        Task<bool> DeleteTechnicalIndicatorAsync(int id);
        Task<int> ArchivePastTechnicalIndicatorsAsync(CancellationToken ct = default);
    }
}
