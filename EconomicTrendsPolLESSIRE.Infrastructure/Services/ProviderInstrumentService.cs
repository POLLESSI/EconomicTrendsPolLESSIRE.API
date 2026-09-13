using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class ProviderInstrumentService : IProviderInstrumentService
    {
    #nullable disable
        private readonly IProviderInstrumentRepository _providerInstrumentRepository;
        private readonly HttpClient _http;
        private readonly IHubContext<MarketDataHub> _marketDataHub;
        private readonly ILogger<ProviderInstrumentService> _logger;

        public ProviderInstrumentService(IProviderInstrumentRepository providerInstrumentRepository, HttpClient http, IHubContext<MarketDataHub> marketDataHub, ILogger<ProviderInstrumentService> logger)
        {
            _providerInstrumentRepository = providerInstrumentRepository;
            _http = http;
            _marketDataHub = marketDataHub;
            _logger = logger;
        }

        public Task<bool> DeleteProviderInstrumentAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProviderInstrument>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrumentDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrument?> GetProviderInstrumentByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrumentDTO?> SaveAsync(ProviderInstrumentDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrument?> SaveProviderInstrumentAsync(ProviderInstrument providerinstrum, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
