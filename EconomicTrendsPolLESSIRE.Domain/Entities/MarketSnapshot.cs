namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public class MarketSnapshot
    {
        public long InstrumentId { get; set; }
        public decimal LastPrice { get; set; } = 0;
        public decimal BidPrice { get; set; } = 0;
        public decimal AskPrice { get; set; } 
        public decimal OpenPrice { get; set; } = 0;
        public decimal HighPrice { get; set; } = 0;
        public decimal LowPrice { get; set; } = 0;
        public decimal PreviousClose {  get; set; } = 0;
        public decimal Volume { get; set; } = 0;
        public int LastProviderId { get; set; }
        public DateTime MarketTimestampUtc { get; set; } 
        public DateTime ReceivedAtUtc { get; set; }
        public bool Active { get; set; }
    }
}





























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.