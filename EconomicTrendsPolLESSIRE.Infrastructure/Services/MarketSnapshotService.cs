using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

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

        public Task<bool> DeleteMarketSnapshotAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<MarketSnapshot>> IMarketSnapshotService.GetAllAsync(int limit, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<MarketSnapshotDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<MarketSnapshot> IMarketSnapshotService.GetMarketSnapshotByIdAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<MarketSnapshotDTO?> SaveAsync(MarketSnapshotDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<MarketSnapshot?> SaveMarketSnapshotAsync(MarketSnapshot marketsnsht, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

    }
}
