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

        public async Task<bool> DeleteMarketTradeAsync(long id, CancellationToken ct = default)
        {
            var ok = await _marketTradeRepository.DeleteMarketTradeAsync(id);

            if (ok)
            {
                await _marketDataHub.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketTradeArchived, id, ct);
            }

            return ok;
        }

        public async Task<IEnumerable<MarketTrade>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            return await _marketTradeRepository.GetAllMarketTradeAsync(limit, ct);
        }

        public async Task<MarketTradeDTO?> GetByIdAsync(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The market trade ID must be greater than zero.", nameof(id));
            }

            var marketTradeEntity = await _marketTradeRepository.GetMarketTradeByIdAsync(id);

            if (marketTradeEntity == null || !marketTradeEntity.Active)
            {
                return null;
            }

            return marketTradeEntity.MapToMarketTradeDTO();
        }

        public async Task<MarketTrade?> GetMarketTradeByIdAsync(long id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The market trade ID must be greater than zero.", nameof(id));
            }

            var marketTradeEntity = await _marketTradeRepository.GetMarketTradeByIdAsync(id, ct);

            if (marketTradeEntity == null || !marketTradeEntity.Active)
            {
                return null;
            }

            return marketTradeEntity;
        }

        public Task<MarketTradeDTO?> SaveAsync(MarketTradeDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<MarketTrade?> SaveMarketTradeAsync(MarketTrade markettrd, CancellationToken ct = default)
        {
            if (!Validators.IsFutureOrToday(markettrd.ReceivedAtUtc))
            {
                throw new ValidationException("The date must be today or in the future.");
            }

            return await _marketTradeRepository.SaveMarketTradeAsync(markettrd);
        }
    }
}





































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.