using EconomicTrendsPolLESSIRE.Application.Common;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using EconomicTrendsPolLESSIRE.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

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

        public async Task<bool> DeleteProviderInstrumentAsync(int id, CancellationToken ct = default)
        {
            var ok = await _providerInstrumentRepository.DeleteProviderInstrumentAsync(id);

            if (ok)
            {
                await _marketDataHub.Clients.All.SendAsync(MarketHubMethods.ToClient.ProviderInstrumentArchived, id, ct);
            }

            return ok;
        }

        public async Task<IEnumerable<ProviderInstrument>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            return await _providerInstrumentRepository.GetAllProviderInstrumentAsync(limit, ct);
        }

        public async Task<ProviderInstrumentDTO?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The provider instrument ID must be greater than zero.", nameof(id));
            }

            var providerInstrumentEntity = await _providerInstrumentRepository.GetProviderInstrumentByIdAsync(id);

            if (providerInstrumentEntity == null || !providerInstrumentEntity.Active)
            {
                return null;
            }

            return providerInstrumentEntity.MapToProviderInstrumentDTO();
        }

        public async Task<ProviderInstrument?> GetProviderInstrumentByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The provider instrument ID must be greater than zero.", nameof(id));
            }

            var providerInstrumentEntity = await _providerInstrumentRepository.GetProviderInstrumentByIdAsync(id, ct);

            if (providerInstrumentEntity == null || !providerInstrumentEntity.Active)
            {
                return null;
            }

            return providerInstrumentEntity;
        }

        public Task<ProviderInstrumentDTO?> SaveAsync(ProviderInstrumentDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ProviderInstrument?> SaveProviderInstrumentAsync(ProviderInstrument providerinstrum, CancellationToken ct = default)
        {
            return await _providerInstrumentRepository.SaveProviderInstrumentAsync(providerinstrum);
        }
    }
}















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.