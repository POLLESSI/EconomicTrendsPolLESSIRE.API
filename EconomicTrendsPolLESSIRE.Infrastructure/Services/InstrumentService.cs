using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

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

        public Task<bool> DeleteInstrumentAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Instrument>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<InstrumentDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Instrument?> GetInstrumentByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<InstrumentDTO?> SaveAsync(InstrumentDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<Instrument?> SaveInstrumentAsync(Instrument instrument, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
