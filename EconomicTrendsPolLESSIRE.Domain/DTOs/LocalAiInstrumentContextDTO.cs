namespace EconomicTrendsPolLESSIRE.Domain.DTOs
{
    public sealed class LocalAiInstrumentContextDTO
    {
        public long Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int AssetClass { get; set; }
        public string? ExchangeCode { get; set; }
        public string? CurrencyCode { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public bool Active { get; set; }
    }
}












































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.