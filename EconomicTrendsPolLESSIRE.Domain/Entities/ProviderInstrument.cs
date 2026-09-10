namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public class ProviderInstrument
    {
    #nullable disable
        public int ProviderId { get; set; }
        public int InstrumentId { get; set; }
        public string ProviderSymbol { get; set; }
        public bool Realtime { get; set; }
        public int DelaySeconds { get; set; }
        public bool Active { get; set; }
    }
}
