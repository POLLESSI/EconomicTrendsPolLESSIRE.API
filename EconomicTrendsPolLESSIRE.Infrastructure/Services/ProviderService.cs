using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class ProviderService : IProviderService
    {
    #nullable disable
        private readonly IProviderRepository _providerRepository;
        private readonly HttpClient _http;
        private readonly IHubContext<MarketDataHub> _marketDataHub;
        private readonly ILogger<ProviderService> _logger;

        public ProviderService(IProviderRepository providerRepository, HttpClient http, IHubContext<MarketDataHub> marketDataHub, ILogger<ProviderService> logger)
        {
            _providerRepository = providerRepository;
            _http = http;
            _marketDataHub = marketDataHub;
            _logger = logger;
        }

        public Task<bool> DeleteProviderAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Provider>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Provider?> GetProviderByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderDTO?> SaveAsync(ProviderDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<Provider?> SaveProviderAsync(Provider provider, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
