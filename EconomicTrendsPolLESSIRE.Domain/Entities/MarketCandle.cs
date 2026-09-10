namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public class MarketCandle
    {
        public int InstrumentId { get; set; }
        public int IntervalCode { get; set; }
        public DateTime OpenTimeUtc { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal Volume { get; set; }
        public decimal VWAP { get; set; }
        public int TradeCount { get; set; }
        public bool IsFinal { get; set; }
        public bool Active { get; set; }
    }
}
