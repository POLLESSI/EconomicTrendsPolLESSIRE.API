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

        public async Task<bool> DeleteMarketQuoteAsync(int id, CancellationToken ct = default)
        {
            var ok = await _marketQuoteRepository.DeleteMarketQuoteAsync(id);

            if (ok)
            {
                await _marketDataHub.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketQuoteArchived, id, ct);
            }

            return ok;
        }

        public async Task<IEnumerable<MarketQuote>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            return await _marketQuoteRepository.GetAllMarketQuoteAsync(limit, ct);
        }

        public async Task<MarketQuoteDTO?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The market quote ID must be greater than zero.", nameof(id));
            }

            var marketQuoteEntity = await _marketQuoteRepository.GetMarketQuoteByIdAsync(id);

            if (marketQuoteEntity == null || !marketQuoteEntity.Active)
            {
                return null;
            }

            return marketQuoteEntity.MapToMarketQuoteDTO();
        }

        public async Task<MarketQuote?> GetMarketQuoteByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The market quote ID must be greater than zero.", nameof(id));
            }

            var marketQuoteEntity = await _marketQuoteRepository.GetMarketQuoteByIdAsync(id, ct);

            if (marketQuoteEntity == null || !marketQuoteEntity.Active)
            {
                return null;
            }

            return marketQuoteEntity;
        }

        public Task<MarketQuoteDTO?> SaveAsync(MarketQuoteDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<MarketQuote?> SaveMarketQuoteAsync(MarketQuote marketqt, CancellationToken ct = default)
        {
            if (!Validators.IsFutureOrToday(marketqt.ReceivedAtUtc))
            {
                throw new ValidationException("The date must be today or in the future.");
            }

            return await _marketQuoteRepository.SaveMarketQuoteAsync(marketqt);
        }
    }
}










































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.