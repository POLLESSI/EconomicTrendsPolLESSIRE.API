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

        public async Task<bool> DeleteTechnicalIndicatorAsync(long instrumentId, int intervalCode, int indicatorType, DateTime timestampUtc, CancellationToken ct = default)
        {
            var ok = await _technicalIndicatorRepository.DeleteTechnicalIndicatorAsync( instrumentId, intervalCode, indicatorType, timestampUtc, ct);

            if (ok)
            {
                await _marketDataHub.Clients.All.SendAsync(MarketHubMethods.ToClient.TechnicalIndicatorArchived,
                new
                {
                    InstrumentId = instrumentId,
                    IntervalCode = intervalCode,
                    IndicatorType = indicatorType,
                    TimestampUtc = timestampUtc
                },
                ct);
            }

            return ok;
        }

        public async Task<IEnumerable<TechnicalIndicator>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            return await _technicalIndicatorRepository.GetAllTechnicalIndicatorAsync(limit, ct);
        }

        public async Task<TechnicalIndicatorDTO?> GetByIdAsync(long instrumentId, int intervalCode, int indicatorType, DateTime timestampUtc, CancellationToken ct = default)
        {
            if (instrumentId <= 0)
            {
                throw new ArgumentException("The technical indicator ID must be greater than zero.", nameof(instrumentId));
            }

            var technicalIndicatorEntity = await _technicalIndicatorRepository.GetTechnicalIndicatorByIdAsync(instrumentId, intervalCode, indicatorType, timestampUtc, ct);

            if (technicalIndicatorEntity == null || !technicalIndicatorEntity.Active)
            {
                return null;
            }

            return technicalIndicatorEntity.MapToTechnicalIndicatorDTO();
        }

        public async Task<TechnicalIndicator?> GetTechnicalIndicatorByIdAsync(long instrumentId, int intervalCode, int indicatorType, DateTime timestampUtc, CancellationToken ct = default)
        {
            if (instrumentId <= 0)
            {
                throw new ArgumentException("The technical indicator ID must be greater than zero.", nameof(instrumentId));
            }

            var technicalIndicatorEntity = await _technicalIndicatorRepository.GetTechnicalIndicatorByIdAsync(instrumentId, intervalCode, indicatorType, timestampUtc, ct);

            if (technicalIndicatorEntity == null || !technicalIndicatorEntity.Active)
            {
                return null;
            }

            return technicalIndicatorEntity;
        }

        public Task<TechnicalIndicatorDTO?> SaveAsync(TechnicalIndicatorDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<TechnicalIndicator?> SaveTechnicalIndicatorAsync(TechnicalIndicator technicalIndic, CancellationToken ct = default)
        {
            if (!Validators.IsFutureOrToday(technicalIndic.TimestampUtc))
            {
                throw new ValidationException("The date must be today or in the future.");
            }

            return await _technicalIndicatorRepository.SaveTechnicalIndicatorAsync(technicalIndic);
        }
    }
}



























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.