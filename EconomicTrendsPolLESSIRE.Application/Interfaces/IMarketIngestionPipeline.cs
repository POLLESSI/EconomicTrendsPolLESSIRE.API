namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMarketIngestionPipeline
    {
        Task RunOnceAsync(CancellationToken ct = default);
    }
}
