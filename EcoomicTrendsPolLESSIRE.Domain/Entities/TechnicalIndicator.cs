namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public class TechnicalIndicator
    {
        public int InstrumentId { get; set; }
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
