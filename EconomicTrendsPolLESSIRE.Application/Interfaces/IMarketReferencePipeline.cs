namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMarketReferencePipeline
    {
        Task SynchronizeAsync(CancellationToken ct = default);
    }
}
