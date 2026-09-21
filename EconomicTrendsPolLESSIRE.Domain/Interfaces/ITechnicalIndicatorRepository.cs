using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface ITechnicalIndicatorRepository
    {
        Task<TechnicalIndicator?> SaveTechnicalIndicatorAsync(TechnicalIndicator technicalindic);
        Task<IEnumerable<TechnicalIndicator>> GetAllTechnicalIndicatorAsync(int limit = 200, CancellationToken ct = default);
        Task<TechnicalIndicator?> GetTechnicalIndicatorByIdAsync(long instrumentId, int intervalCode, int indicatorType, DateTime timestampUtc, CancellationToken ct = default);
        Task<bool> DeleteTechnicalIndicatorAsync(long instrumentId, int intervalCode, int indicatorType, DateTime timestampUtc, CancellationToken ct = default);
        Task<int> ArchivePastTechnicalIndicatorsAsync(CancellationToken ct = default);
    }
}



































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.