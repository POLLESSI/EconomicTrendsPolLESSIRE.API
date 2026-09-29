namespace EconomicTrendsPolLESSIRE.Application.MarketData
{
    public sealed record NormalizedQuote(
       long InstrumentId,
       int ProviderId,
       DateTime TimestampUtc,
       DateTime ReceivedAtUtc,
       decimal BidPrice,
       decimal BidSize,
       decimal AskPrice,
       decimal AskSize);
}






















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.