using Dapper;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class MarketQuoteRepository : IMarketQuoteRepository
    {
    #nullable disable
        private readonly System.Data.IDbConnection _connection;
        private readonly ILogger<MarketQuoteRepository> _logger;

        public MarketQuoteRepository(IDbConnection connection, ILogger<MarketQuoteRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public Task<int> ArchivePastMarketQuotesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMarketQuoteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarketQuote>> GetAllMarketQuoteAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketQuote?> GetMarketQuoteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MarketQuote?> SaveMarketQuoteAsync(MarketQuote marketQt)
        {
            throw new NotImplementedException();
        }
    }
}
