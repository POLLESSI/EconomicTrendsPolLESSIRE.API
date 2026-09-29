namespace EconomicTrendsPolLESSIRE.Application.MarketData
{
    public interface IMarketDataSource
    {
        string Code { get; }

        Task<IReadOnlyCollection<ProviderCatalogItem>> GetCatalogAsync(CancellationToken ct = default);

        Task<MarketDataBatch> GetMarketDataAsync(IReadOnlyCollection<ProviderSubscription> subscriptions, CancellationToken ct = default);
    }
}
