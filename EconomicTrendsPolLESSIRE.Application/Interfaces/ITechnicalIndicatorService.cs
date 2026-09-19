using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface ITechnicalIndicatorService
    {
        Task<TechnicalIndicatorDTO?> GetByIdAsync(int id);
        Task<TechnicalIndicatorDTO?> SaveAsync(TechnicalIndicatorDTO dto);
        Task<TechnicalIndicator?> SaveTechnicalIndicatorAsync(TechnicalIndicator technicalIndic, CancellationToken ct = default);
        Task<IEnumerable<TechnicalIndicator>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<TechnicalIndicator?> GetTechnicalIndicatorByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteTechnicalIndicatorAsync(int id, CancellationToken ct = default);
    }
}





































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.