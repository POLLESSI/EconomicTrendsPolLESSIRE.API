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
    public class InstrumentService : IInstrumentService
    {
    #nullable disable

        private readonly IInstrumentRepository _instrumentRepository;
        private readonly HttpClient _http;
        private readonly IHubContext<MarketDataHub> _marketDataHub;
        private readonly ILogger<InstrumentService> _logger;

        public InstrumentService(IInstrumentRepository instrumentRepository, HttpClient http, IHubContext<MarketDataHub> marketDataHub, ILogger<InstrumentService> logger)
        {
            _instrumentRepository = instrumentRepository;
            _http = http;
            _marketDataHub = marketDataHub;
            _logger = logger;
        }

        public async Task<bool> DeleteInstrumentAsync(long id, CancellationToken ct = default)
        {
            var ok = await _instrumentRepository.DeleteInstrumentAsync(id);

            if (ok)
            {
                await _marketDataHub.Clients.All.SendAsync(MarketHubMethods.ToClient.InstrumentArchived, id, ct);
            }

            return ok;
        }

        public async Task<IEnumerable<Instrument>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            return await _instrumentRepository.GetAllInstrumentAsync(limit, ct);
        }

        public async Task<InstrumentDTO?> GetByIdAsync(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The instrument ID must be greater than zero.", nameof(id));
            }

            var instrumentEntity = await _instrumentRepository.GetInstrumentByIdAsync(id);

            if (instrumentEntity == null || !instrumentEntity.Active)
            {
                return null;
            }

            return instrumentEntity.MapToInstrumentDTO();
        }

        public async Task<Instrument?> GetInstrumentByIdAsync(long id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                throw new ArgumentException("The instrument ID must be greater than zero.", nameof(id));
            }

            var instrumentEntity = await _instrumentRepository.GetInstrumentByIdAsync(id, ct);

            if (instrumentEntity == null || !instrumentEntity.Active)
            {
                return null;
            }

            return instrumentEntity;
        }

        public Task<InstrumentDTO?> SaveAsync(InstrumentDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<Instrument?> SaveInstrumentAsync(Instrument instrument, CancellationToken ct = default)
        {
            if (!Validators.IsFutureOrToday(instrument.CreatedAtUtc))
            {
                throw new ValidationException("The date must be today or in the future.");
            }

            return await _instrumentRepository.SaveInstrumentAsync(instrument);
        }
    }
}














































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.