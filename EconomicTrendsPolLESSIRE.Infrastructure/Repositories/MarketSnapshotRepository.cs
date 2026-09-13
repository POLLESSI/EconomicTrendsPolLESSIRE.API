using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;
using System.Data.Common;
using System.Data;
using Dapper;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class MarketSnapshotRepository : IMarketSnapshotRepository
    {
    #nullable disable
        private readonly System.Data.IDbConnection _connection;
        private readonly ILogger<MarketSnapshotRepository> _logger;

        public MarketSnapshotRepository(IDbConnection connection, ILogger<MarketSnapshotRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public Task<int> ArchivePastMarketSnapshotsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMarketSnapshotAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarketSnapshot>> GetAllMarketSnapshotAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketSnapshot?> GetMarketSnapshotByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<MarketSnapshot> SaveMarketSnapshotAsync(MarketSnapshot marketSnpsht)
        {
            throw new NotImplementedException();
        }
    }
}
