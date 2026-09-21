using EconomicTrendsPolLESSIRE.Application.Extensions;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
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

        public async Task<bool> DeleteProviderInstrumentAsync(int providerId, long instrumentId, CancellationToken ct = default)
        {
            ValidateKey(providerId, instrumentId);

            var ok = await _providerInstrumentRepository.DeleteProviderInstrumentAsync(providerId, instrumentId);

            if (ok)
            {
                await _marketDataHub.Clients.All.SendAsync(MarketHubMethods.ToClient.ProviderInstrumentArchived, new { ProviderId = providerId, InstrumentId = instrumentId }, ct);
            }

            return ok;
        }

        public async Task<IEnumerable<ProviderInstrument>> GetAllAsync(
            int limit = 500,
            CancellationToken ct = default)
        {
            return await _providerInstrumentRepository
                .GetAllProviderInstrumentAsync(limit, ct);
        }

        public async Task<ProviderInstrumentDTO?> GetByIdAsync(int providerId, long instrumentId, CancellationToken ct = default)
        {
            ValidateKey(providerId, instrumentId);

            var entity = await _providerInstrumentRepository.GetProviderInstrumentByIdAsync(providerId, instrumentId, ct);

            if (entity == null || !entity.Active)
            {
                return null;
            }

            return entity.MapToProviderInstrumentDTO();
        }

        public async Task<ProviderInstrument?>GetProviderInstrumentByIdAsync(int providerId, long instrumentId, CancellationToken ct = default)
        {
            ValidateKey(providerId, instrumentId);

            var entity = await _providerInstrumentRepository.GetProviderInstrumentByIdAsync(providerId, instrumentId, ct);

            if (entity == null || !entity.Active)
            {
                return null;
            }

            return entity;
        }

        public Task<ProviderInstrumentDTO?> SaveAsync(ProviderInstrumentDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ProviderInstrument?> SaveProviderInstrumentAsync(ProviderInstrument providerInstrument, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(providerInstrument);

            ValidateKey(providerInstrument.ProviderId, providerInstrument.InstrumentId);

            return await _providerInstrumentRepository.SaveProviderInstrumentAsync(providerInstrument);
        }

        private static void ValidateKey(int providerId, long instrumentId)
        {
            if (providerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(providerId), "The provider ID must be greater than zero.");
            }

            if (instrumentId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(instrumentId), "The instrument ID must be greater than zero.");
            }
        }
    }
}















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.