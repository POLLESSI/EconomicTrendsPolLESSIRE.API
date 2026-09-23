using EconomicTrendsPolLESSIRE.Domain.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class LocalAiDataRepository : ILocalAiDataRepository
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<LocalAiDataRepository> _logger;

        public LocalAiDataRepository(IDbConnection connection, ILogger<LocalAiDataRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public Task<IEnumerable<LocalAiInstrumentContextDTO>> GetNearbyInstrumentsAsync(double latitude, double longitude, double radiusKm, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocalAiMarketCandleContextDTO>> GetNearbyMarketCandlesAsync(double latitude, double longitude, DateTime dateFrom, DateTime dateToExclusive, double radiusKm, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocalAiMarketQuoteContextDTO>> GetNearbyMarketQuoteAsync(double latitude, double longitude, DateTime dateFrom, DateTime dateToExclusive, double radiusKm, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocalAiMarketSnapshotContextDTO>> GetNearbyMarketSnapshotAsync(double latitude, double longitude, DateTime targetDate, double radiusKm, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocalAiMarketTradeContextDTO>> GetNearbyMarketTradeContextAsync(double latitude, double longitude, DateTime dateFrom, DateTime dateToExclusive, double radiusKm, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocalAiProviderContextDTO>> GetNearbyProviderAsync(double latitude, double longitude, DateTime dateFrom, DateTime dateToExclusive, double radiusKm, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocalAiProviderInstrumentContextDTO>> GetNearbyProviderInstrumentsAsync(double latitude, double longitude, double radiusKm, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocalAiTechnicalIndicatorContextDTO>> GetNearbyTechnicalIndicatorsAsync(double latitude, double longitude, double radiusKm, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocalAiUserMessageContextDTO>> GetNearbyUserMessagesAsync(double latitude, double longitude, double radiusKm, DateTime sinceUtc, int limit = 10, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<LocalAiInstrumentContextDTO>> SearchInstrumentsByKeywordsAsync(string userPrompt, int limit = 10, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}

































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.