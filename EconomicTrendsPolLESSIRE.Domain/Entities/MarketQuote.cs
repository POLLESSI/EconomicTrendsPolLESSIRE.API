namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public class MarketQuote
    {
        public int Id { get; set; }
        public long InstrumentId { get; set; }
        public int ProviderId { get; set; }
        public DateTime TimestampUtc { get; set; }
        public DateTime ReceivedAtUtc { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidSize { get; set; } = 0;
        public decimal AskPrice { get; set; }
        public decimal AskSize { get; set; } = 0;
        public bool Active { get; set; }
    }
}

















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.