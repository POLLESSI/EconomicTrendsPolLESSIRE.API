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

        public async Task<bool> DeleteProviderAsync(int id, CancellationToken ct = default)
        {
            var ok = await _providerRepository.DeleteProviderAsync(id);

            if (ok)
            {
                await _marketDataHub.Clients.All.SendAsync(MarketHubMethods.ToClient.ProviderArchived, id, ct);
            }

            return ok;
        }

        public async Task<IEnumerable<Provider>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            return await _providerRepository.GetAllProviderAsync(limit, ct);
        }

        public async Task<ProviderDTO?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The provider ID must be greater than zero.", nameof(id));
            }

            var providerEntity = await _providerRepository.GetProviderByIdAsync(id);

            if (providerEntity == null || !providerEntity.Active)
            {
                return null;
            }

            return providerEntity.MapToProviderDTO();
        }

        public async Task<Provider?> GetProviderByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The provider ID must be greater than zero.", nameof(id));
            }

            var providerEntity = await _providerRepository.GetProviderByIdAsync(id, ct);

            if (providerEntity == null || !providerEntity.Active)
            {
                return null;
            }

            return providerEntity;
        }

        public Task<ProviderDTO?> SaveAsync(ProviderDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<Provider?> SaveProviderAsync(Provider provider, CancellationToken ct = default)
        {
            if (!Validators.IsFutureOrToday(provider.CreatedAtUtc))
            {
                throw new ValidationException("The date must be today or in the future.");
            }

            return await _providerRepository.SaveProviderAsync(provider);
        }
    }
}





































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.