using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class MarketCandleService : IMarketCandleService
    {
        public Task<bool> DeleteMarketCandleAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarketCandle>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketCandleDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MarketCandle?> GetMarketCandleByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketCandleDTO?> SaveAsync(MarketCandleDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<MarketCandle?> SaveInstrumentAsync(MarketCandle marketcndl, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
