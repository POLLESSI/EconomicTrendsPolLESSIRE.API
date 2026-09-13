using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class MarketCandleRepository : IMarketCandleRepository
    {
    #nullable disable
        private readonly System.Data.IDbConnection _connection;
        private readonly ILogger<MarketCandleRepository> _logger;

        public MarketCandleRepository(IDbConnection connection, ILogger<MarketCandleRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public Task<int> ArchivePastMarketCandlesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMarketCandleAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarketCandle>> GetAllMarketCandleAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketCandle?> GetMarketCandleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MarketCandle?> SaveMarketCandleAsync(MarketCandle marketCdl)
        {
            throw new NotImplementedException();
        }
    }
}
