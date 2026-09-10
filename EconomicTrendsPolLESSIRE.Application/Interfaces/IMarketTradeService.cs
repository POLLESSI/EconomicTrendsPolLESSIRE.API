using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMarketTradeService
    {
        Task<MarketTradeDTO?> GetByIdAsync(int id);
        Task<MarketTradeDTO?> SaveAsync(MarketTradeDTO dto);
        Task<MarketTrade?> SaveMarketTradeAsync(MarketTrade markettrd, CancellationToken ct = default);
        Task<IEnumerable<MarketTrade>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<MarketTrade?> GetMarketTradeByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteMarketTradeAsync(int id, CancellationToken ct = default);
    }
}
