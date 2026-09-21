using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface ITechnicalIndicatorService
    {
        Task<TechnicalIndicatorDTO?> GetByIdAsync(long instrumentId, int intervalCode, int indicatorType, DateTime timestampUtc, CancellationToken ct = default);
        Task<TechnicalIndicatorDTO?> SaveAsync(TechnicalIndicatorDTO dto);
        Task<TechnicalIndicator?> SaveTechnicalIndicatorAsync(TechnicalIndicator technicalIndic, CancellationToken ct = default);
        Task<IEnumerable<TechnicalIndicator>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<TechnicalIndicator?> GetTechnicalIndicatorByIdAsync(long instrumentId, int intervalCode, int indicatorType, DateTime timestampUtc, CancellationToken ct = default);
        Task<bool> DeleteTechnicalIndicatorAsync(long instrumentId, int intervalCode, int indicatorType, DateTime timestampUtc, CancellationToken ct = default);
    }
}





































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.