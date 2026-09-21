namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public class MarketTrade
    {
        public long Id { get; set; }
        public long InstrumentId { get; set; }
        public int ProviderId { get; set; }
        public DateTime TimestampUtc { get; set; }
        public DateTime ReceivedAtUtc { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public long? SequenceNumber { get; set; }
        public bool Active { get; set; }
    }
}





























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.