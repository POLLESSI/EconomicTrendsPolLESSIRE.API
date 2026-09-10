using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMarketQuoteService
    {
        Task<MarketQuoteDTO?> GetByIdAsync(int id);
        Task<MarketQuoteDTO?> SaveAsync(MarketQuoteDTO dto);
        Task<MarketQuote?> SaveMarketQuoteAsync(MarketQuote marketqt, CancellationToken ct = default);
        Task<IEnumerable<MarketQuote>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<MarketQuote?> GetMarketQuoteByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteMarketQuoteAsync(int id, CancellationToken ct = default);
    }
}
