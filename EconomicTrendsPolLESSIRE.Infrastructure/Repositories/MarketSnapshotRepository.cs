using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class MarketSnapshotRepository : IMarketSnapshotRepository
    {
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

        public Task<MarketSnapshot?> SaveMarketSnapshotAsync(Instrument instrument)
        {
            throw new NotImplementedException();
        }
    }
}
