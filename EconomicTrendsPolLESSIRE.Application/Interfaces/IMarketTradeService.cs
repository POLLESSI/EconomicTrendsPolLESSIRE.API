using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMarketTradeService
    {
        Task<MarketTradeDTO?> GetByIdAsync(long id);
        Task<MarketTradeDTO?> SaveAsync(MarketTradeDTO dto);
        Task<MarketTrade?> SaveMarketTradeAsync(MarketTrade markettrd, CancellationToken ct = default);
        Task<IEnumerable<MarketTrade>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<MarketTrade?> GetMarketTradeByIdAsync(long id, CancellationToken ct = default);
        Task<bool> DeleteMarketTradeAsync(long id, CancellationToken ct = default);
    }
}














































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.