using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class MarketQuoteService : IMarketQuoteService
    {
    #nullable disable
        private readonly IMarketQuoteRepository _marketQuoteRepository;
        private readonly HttpClient _http;
        private readonly IHubContext<MarketDataHub> _marketDataHub;
        private readonly ILogger<MarketQuoteService> _logger;

        public MarketQuoteService(IMarketQuoteRepository marketQuoteRepository, HttpClient http, IHubContext<MarketDataHub> marketDataHub, ILogger<MarketQuoteService> logger)
        {
            _marketQuoteRepository = marketQuoteRepository;
            _http = http;
            _marketDataHub = marketDataHub;
            _logger = logger;
        }

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
