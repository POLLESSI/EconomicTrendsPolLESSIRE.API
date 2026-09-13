using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Infrastructure.Repositories;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class TechnicalIndicatorService : ITechnicalIndicatorService
    {
    #nullable disable
        private readonly ITechnicalIndicatorRepository _technicalIndicatorRepository;
        private readonly HttpClient _http;
        private readonly IHubContext<MarketDataHub> _marketDataHub;
        private readonly ILogger<TechnicalIndicatorService> _logger;

        public TechnicalIndicatorService(ITechnicalIndicatorRepository technicalIndicatorRepository, HttpClient http, IHubContext<MarketDataHub> marketDataHub, ILogger<TechnicalIndicatorService> logger)
        {
            _technicalIndicatorRepository = technicalIndicatorRepository;
            _http = http;
            _marketDataHub = marketDataHub;
            _logger = logger;
        }

        public Task<bool> DeleteTechnicalIndicatorAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TechnicalIndicator>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicatorDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicator?> GetTechnicalIndicatorByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicatorDTO?> SaveAsync(TechnicalIndicatorDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicator?> SaveTechnicalIndicatorAsync(TechnicalIndicator technicalIndic, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
