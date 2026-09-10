using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class MarketQuoteService : IMarketQuoteService
    {
        public Task<bool> DeleteMarketQuoteAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarketQuote>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketQuoteDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MarketQuote?> GetMarketQuoteByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketQuoteDTO?> SaveAsync(MarketQuoteDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<MarketQuote?> SaveMarketQuoteAsync(MarketQuote marketqt, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
