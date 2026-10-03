using EconomicTrendsPolLESSIRE.Application.MarketData;

namespace EconomicTrendsPolLESSIRE.Infrastructure.MarketData
{
    public sealed class MockMarketDataSource : IMarketDataSource
    {
        public string Code => "MOCK";

        public Task<IReadOnlyCollection<ProviderCatalogItem>>GetCatalogAsync(CancellationToken ct = default)
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

        public Task<MarketDataBatch> GetMarketDataAsync(IReadOnlyCollection<ProviderSubscription> subscriptions, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var now = DateTime.UtcNow;

            var candleOpenTime =
                new DateTime(
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    0,
                    DateTimeKind.Utc);

            var trades = new List<NormalizedTrade>(subscriptions.Count);
            var quotes = new List<NormalizedQuote>(subscriptions.Count);
            var candles = new List<NormalizedCandle>(subscriptions.Count);

            foreach (var subscription in subscriptions)
            {
                ct.ThrowIfCancellationRequested();

                var price = Random.Shared.Next(10000, 50000) / 100m;

                var quantity = Random.Shared.Next(1, 1000);

                // ---------------------------------------------
                // TRADE
                // ---------------------------------------------

                trades.Add(
                    new NormalizedTrade(
                        subscription.InstrumentId,
                        subscription.ProviderId,
                        now,
                        now,
                        price,
                        quantity,
                        null));

                // ---------------------------------------------
                // QUOTE
                // ---------------------------------------------

                var spread = Random.Shared.Next(1, 20) / 100m;
                var bidPrice = Math.Max(0.01m, price - spread);
                var askPrice = price + spread;
                var bidSize = Random.Shared.Next(1, 1000);
                var askSize = Random.Shared.Next(1, 1000);
                        
                quotes.Add(
                    new NormalizedQuote(
                        subscription.InstrumentId,
                        subscription.ProviderId,
                        now,
                        now,
                        bidPrice,
                        bidSize,
                        askPrice,
                        askSize));

                // ---------------------------------------------
                // CANDLE 1 MINUTE
                // ---------------------------------------------

                var openOffset = Random.Shared.Next(-100, 101) / 100m;
                var closeOffset = Random.Shared.Next(-100, 101) / 100m;
                var openPrice = Math.Max(0.01m, price + openOffset);
                var closePrice = Math.Max(0.01m, price + closeOffset);
                var highExtra = Random.Shared.Next(0, 101) / 100m;
                var lowExtra = Random.Shared.Next(0, 101) / 100m;
                var highPrice = Math.Max(Math.Max(openPrice, closePrice), price) + highExtra;
                var lowPrice = Math.Max(0.01m, Math.Min(Math.Min( openPrice, closePrice), price) - lowExtra);
                var volume = Random.Shared.Next(1000, 50000);
                var tradeCount = Random.Shared.Next(1, 500);

                var vwap = (openPrice + highPrice + lowPrice + closePrice) / 4m;

                candles.Add(
                    new NormalizedCandle(
                        subscription.InstrumentId,
                        1,
                        candleOpenTime,
                        openPrice,
                        highPrice,
                        lowPrice,
                        closePrice,
                        volume,
                        vwap,
                        tradeCount,
                        false));
            }

            return Task.FromResult(new MarketDataBatch(trades, quotes, candles));
        }
    }
}









































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.