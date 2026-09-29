namespace EconomicTrendsPolLESSIRE.Application.MarketData
{
    public sealed record NormalizedCandle(
        long InstrumentId,
        int IntervalCode,
        DateTime OpenTimeUtc,
        decimal OpenPrice,
        decimal HighPrice,
        decimal LowPrice,
        decimal ClosePrice,
        decimal Volume,
        decimal VWAP,
        int TradeCount,
        bool IsFinal);
}































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.