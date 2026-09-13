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
    public class MarketTradeRepository : IMarketTradeRepository
    {
        #nullable disable
        private readonly System.Data.IDbConnection _connection;
        private readonly ILogger<MarketTradeRepository> _logger;

        public MarketTradeRepository(IDbConnection connection, ILogger<MarketTradeRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public Task<int> ArchivePastMarketTradesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMarketTradeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarketTrade>> GetAllMarketTradeAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketTrade?> GetMarketTradeByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MarketTrade?> SaveMarketTradeAsync(MarketTrade markettrd)
        {
            throw new NotImplementedException();
        }
    }
}
