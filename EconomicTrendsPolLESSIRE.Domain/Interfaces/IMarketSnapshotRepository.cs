using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IMarketSnapshotRepository
    {
        Task<MarketSnapshot?> SaveMarketSnapshotAsync(MarketSnapshot marketSnpsht);
        Task<IEnumerable<MarketSnapshot>> GetAllMarketSnapshotAsync(int limit = 200, CancellationToken ct = default);
        Task<MarketSnapshot?> GetMarketSnapshotByIdAsync(int id);
        Task<bool> DeleteMarketSnapshotAsync(int id);
        Task<int> ArchivePastMarketSnapshotsAsync(CancellationToken ct = default);
    }
}
