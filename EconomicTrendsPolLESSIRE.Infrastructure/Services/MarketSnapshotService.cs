using EconomicTrendsPolLESSIRE.Application.Common;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Application.Extensions;
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
    public class MarketSnapshotService : IMarketSnapshotService
    {
    #nullable disable
        private readonly IMarketSnapshotRepository _marketSnapshotRepository;
        private readonly HttpClient _http;
        private readonly IHubContext<MarketDataHub> _marketDataHub;
        private readonly ILogger<MarketSnapshotService> _logger;

        public MarketSnapshotService(IMarketSnapshotRepository marketSnapshotRepository, HttpClient http, IHubContext<MarketDataHub> marketDataHub, ILogger<MarketSnapshotService> logger)
        {
            _marketSnapshotRepository = marketSnapshotRepository;
            _http = http;
            _marketDataHub = marketDataHub;
            _logger = logger;
        }

        public async Task<bool> DeleteMarketSnapshotAsync(int id, CancellationToken ct = default)
        {
            var ok = await _marketSnapshotRepository.DeleteMarketSnapshotAsync(id);

            if (ok)
            {
                await _marketDataHub.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketSnapshotArchived, id, ct);
            }

            return ok;
        }

        async Task<IEnumerable<MarketSnapshot>> IMarketSnapshotService.GetAllAsync(int limit, CancellationToken ct)
        {
            return await _marketSnapshotRepository.GetAllMarketSnapshotAsync(limit, ct);
        }

        public async Task<MarketSnapshotDTO?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The market snapshot ID must be greater than zero.", nameof(id));
            }

            var marketSnapshotEntity = await _marketSnapshotRepository.GetMarketSnapshotByIdAsync(id);

            if (marketSnapshotEntity == null || !marketSnapshotEntity.Active)
            {
                return null;
            }

            return marketSnapshotEntity.MapToMarketSnapshotDTO();
        }

        async Task <MarketSnapshot> IMarketSnapshotService.GetMarketSnapshotByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The market snapshot ID must be greater than zero.", nameof(id));
            }

            var marketSnapshotEntity = await _marketSnapshotRepository.GetMarketSnapshotByIdAsync(id, ct);

            if (marketSnapshotEntity == null || !marketSnapshotEntity.Active)
            {
                return null;
            }

            return marketSnapshotEntity;
        }

        public Task<MarketSnapshotDTO?> SaveAsync(MarketSnapshotDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<MarketSnapshot?> SaveMarketSnapshotAsync(MarketSnapshot marketsnsht, CancellationToken ct = default)
        {
            if (!Validators.IsFutureOrToday(marketsnsht.ReceivedAtUtc))
            {
                throw new ValidationException("The date must be today or in the future.");
            }

            return await _marketSnapshotRepository.SaveMarketSnapshotAsync(marketsnsht);
        }

    }
}










































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.