using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMarketCandleService
    {
        Task<MarketCandleDTO?> GetByIdAsync(int id);
        Task<MarketCandleDTO?> SaveAsync(MarketCandleDTO dto);
        Task<MarketCandle?> SaveMarketCandleAsync(MarketCandle marketcndl, CancellationToken ct = default);
        Task<IEnumerable<MarketCandle>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<MarketCandle?> GetMarketCandleByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteMarketCandleAsync(int id, CancellationToken ct = default);
    }
}





























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.