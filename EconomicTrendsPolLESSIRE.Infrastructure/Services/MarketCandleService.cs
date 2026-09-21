using EconomicTrendsPolLESSIRE.Application.Common;
using EconomicTrendsPolLESSIRE.Application.Extensions;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

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

        public async Task<bool> DeleteMarketCandleAsync(long instrumentId, CancellationToken ct = default)
        {
            var ok = await _marketCandleRepository.DeleteMarketCandleAsync(instrumentId);

            if (ok)
            {
                await _marketDataHub.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketCandleArchived, instrumentId, ct);
            }

            return ok;
        }

        public async Task<IEnumerable<MarketCandle>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            return await _marketCandleRepository.GetAllMarketCandleAsync(limit, ct);
        }

        public async Task<MarketCandleDTO?> GetByIdAsync(long instrumentId)
        {
            if (instrumentId <= 0)
            {
                throw new ArgumentException("The instrument ID must be greater than zero.", nameof(instrumentId));
            }

            var marketCandleEntity = await _marketCandleRepository.GetMarketCandleByIdAsync(instrumentId);

            if (marketCandleEntity == null || !marketCandleEntity.Active)
            {
                return null;
            }

            return marketCandleEntity.MapToMarketCandleDTO();
        }

        public async Task<MarketCandle?> GetMarketCandleByIdAsync(long instrumentId, CancellationToken ct = default)
        {
            if (instrumentId <= 0)
            {
                throw new ArgumentException("The instrument ID must be greater than zero.", nameof(instrumentId));
            }

            var marketCandleEntity = await _marketCandleRepository.GetMarketCandleByIdAsync(instrumentId, ct);

            if (marketCandleEntity == null || !marketCandleEntity.Active)
            {
                return null;
            }

            return marketCandleEntity;
        }

        public Task<MarketCandleDTO?> SaveAsync(MarketCandleDTO dto)
        {
            throw new NotImplementedException();
        }
        public async Task<MarketCandle?> SaveMarketCandleAsync(MarketCandle marketcndl, CancellationToken ct = default)
        {
            if (!Validators.IsFutureOrToday(marketcndl.OpenTimeUtc))
            {
                throw new ValidationException("The date must be today or in the future.");
            }

            return await _marketCandleRepository.SaveMarketCandleAsync(marketcndl);
        }
    }
}














































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.