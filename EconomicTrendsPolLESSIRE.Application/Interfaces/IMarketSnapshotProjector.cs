namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMarketSnapshotProjector
    {
        Task RefreshAsync(long instrumentId, CancellationToken ct = default);
    }
}
