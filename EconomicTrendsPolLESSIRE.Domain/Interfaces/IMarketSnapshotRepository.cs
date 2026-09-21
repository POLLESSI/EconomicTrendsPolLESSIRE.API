using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IMarketSnapshotRepository
    {
        Task<MarketSnapshot?> SaveMarketSnapshotAsync(MarketSnapshot marketSnpsht);
        Task<IEnumerable<MarketSnapshot>> GetAllMarketSnapshotAsync(int limit = 200, CancellationToken ct = default);
        Task<MarketSnapshot?> GetMarketSnapshotByIdAsync(long instrumentId, CancellationToken ct = default);
        Task<bool> DeleteMarketSnapshotAsync(long instrumentId);
        Task<int> ArchivePastMarketSnapshotsAsync(CancellationToken ct = default);
    }
}
































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.