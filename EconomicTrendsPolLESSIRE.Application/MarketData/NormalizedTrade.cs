namespace EconomicTrendsPolLESSIRE.Application.MarketData
{
    public sealed record NormalizedTrade(
        long InstrumentId,
        int ProviderId,
        DateTime TimestampUtc,
        DateTime ReceivedAtUtc,
        decimal Price,
        decimal Quantity,
        long? SequenceNumber);
}














































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.