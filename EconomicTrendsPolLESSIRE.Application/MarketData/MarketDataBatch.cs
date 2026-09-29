namespace EconomicTrendsPolLESSIRE.Application.MarketData
{
    public sealed record MarketDataBatch(
       IReadOnlyCollection<NormalizedTrade> Trades,
       IReadOnlyCollection<NormalizedQuote> Quotes,
       IReadOnlyCollection<NormalizedCandle> Candles);
}
