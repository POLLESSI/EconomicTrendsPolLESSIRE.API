namespace EconomicTrendsPolLESSIRE.Domain.DTOs
{
    public sealed class LocalAiTechnicalIndicatorContextDTO
    {
        public long InstrumentId { get; set; }
        public int IntervalCode { get; set; }
        public DateTime TimestampUtc { get; set; }
        public int IndicatorType { get; set; }
        public decimal Value1 { get; set; }
        public decimal Value2 { get; set; }
        public decimal Value3 { get; set; }
        public byte ParameterHash { get; set; }
        public bool Active { get; set; }
    }
}


































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.