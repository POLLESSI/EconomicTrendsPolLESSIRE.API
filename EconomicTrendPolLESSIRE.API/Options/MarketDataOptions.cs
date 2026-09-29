namespace EconomicTrendsPolLESSIRE.API.Options
{
    public sealed class MarketDataOptions
    {
        public bool Enabled { get; set; } = true;

        public string Provider { get; set; } = "Mock";

        public int ReferenceSyncMinutes { get; set; } = 60;

        public int PollSeconds { get; set; } = 5;

        public int SnapshotSeconds { get; set; } = 5;

        public int IndicatorSeconds { get; set; } = 15;

        public int BatchSize { get; set; } = 500;

        public string[] CandleIntervals { get; set; } = ["1m", "5m"];
    }
}
