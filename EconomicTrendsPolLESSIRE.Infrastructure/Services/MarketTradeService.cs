using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class MarketTradeService : IMarketTradeService
    {
    #nullable disable
        private readonly IMarketTradeRepository _marketTradeRepository;
        private readonly HttpClient _http;
        private readonly IHubContext<MarketDataHub> _marketDataHub;
        private readonly ILogger<MarketTradeService> _logger;

        public MarketTradeService(IMarketTradeRepository marketTradeRepository, HttpClient http, IHubContext<MarketDataHub> marketDataHub, ILogger<MarketTradeService> logger)
        {
            _marketTradeRepository = marketTradeRepository;
            _http = http;
            _marketDataHub = marketDataHub;
            _logger = logger;
        }

        public Task<bool> DeleteMarketTradeAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarketTrade>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketTradeDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MarketTrade?> GetMarketTradeByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketTradeDTO?> SaveAsync(MarketTradeDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<MarketTrade?> SaveMarketTradeAsync(MarketTrade markettrd, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
