namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
#nullable disable
    public class Instrument
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int AssetClass { get; set; }
        public string ExchangeCode { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public bool Active { get; set; }
    }
}
