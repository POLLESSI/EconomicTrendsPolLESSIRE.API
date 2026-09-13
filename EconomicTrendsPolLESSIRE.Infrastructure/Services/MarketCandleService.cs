using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class MarketCandleService : IMarketCandleService
    {
    #nullable disable
        private readonly IMarketCandleRepository _marketCandleRepository;
        private readonly HttpClient _http;
        private readonly IHubContext<MarketDataHub> _marketDataHub;
        private readonly ILogger<MarketCandleService> _logger;

        public MarketCandleService(IMarketCandleRepository marketCandleRepository, HttpClient http, IHubContext<MarketDataHub> marketDataHub, ILogger<MarketCandleService> logger)
        {
            _marketCandleRepository = marketCandleRepository;
            _http = http;
            _marketDataHub = marketDataHub;
            _logger = logger;
        }

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
