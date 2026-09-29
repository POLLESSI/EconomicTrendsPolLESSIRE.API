using EconomicTrendsPolLESSIRE.Application.MarketData;

namespace EconomicTrendsPolLESSIRE.Infrastructure.MarketData
{
    public sealed class MockMarketDataSource
        : IMarketDataSource
    {
        public string Code => "MOCK";

        public Task<IReadOnlyCollection<ProviderCatalogItem>>
            GetCatalogAsync(
                CancellationToken ct = default)
        {
            IReadOnlyCollection<ProviderCatalogItem> result =
            [
                new(
                    "AAPL",
                    "Apple Inc.",
                    0,
                    "NASDAQ",
                    "USD",
                    "AAPL",
                    true,
                    0),

                new(
                    "MSFT",
                    "Microsoft Corporation",
                    0,
                    "NASDAQ",
                    "USD",
                    "MSFT",
                    true,
                    0),

                new(
                    "NVDA",
                    "NVIDIA Corporation",
                    0,
                    "NASDAQ",
                    "USD",
                    "NVDA",
                    true,
                    0)
            ];

            return Task.FromResult(result);
        }

        public Task<MarketDataBatch>
            GetMarketDataAsync(
                IReadOnlyCollection<ProviderSubscription> subscriptions,
                CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;

            var trades =
                subscriptions
                    .Select(x =>
                        new NormalizedTrade(
                            x.InstrumentId,
                            x.ProviderId,
                            now,
                            now,
                            Random.Shared.Next(
                                10000,
                                50000) / 100m,
                            Random.Shared.Next(
                                1,
                                1000),
                            null))
                    .ToArray();

            return Task.FromResult(
                new MarketDataBatch(
                    trades,
                    [],
                    []));
        }
    }
}









































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.