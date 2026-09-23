namespace EconomicTrendsPolLESSIRE.Domain.DTOs
{
    public sealed class LocalAiContextDTO
    {
        public string UserPrompt { get; set; } = string.Empty;

        // =====================================================
        // REQUEST CONTEXT
        // =====================================================

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public double RadiusKm { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateToExclusive { get; set; }

        public bool HasCoordinates => Latitude.HasValue && Longitude.HasValue;

        // =====================================================
        // INSTRUMENTS
        // =====================================================

        public IReadOnlyList<LocalAiInstrumentContextDTO> Instruments { get; set; } = Array.Empty<LocalAiInstrumentContextDTO>();
        public IReadOnlyList<LocalAiInstrumentContextDTO> KeywordMatchedInstruments { get; set; } = Array.Empty<LocalAiInstrumentContextDTO>();

        // =====================================================
        // MARKET DATA
        // =====================================================

        public IReadOnlyList<LocalAiMarketCandleContextDTO> MarketCandles { get; set; } = Array.Empty<LocalAiMarketCandleContextDTO>();
        public IReadOnlyList<LocalAiMarketQuoteContextDTO> MarketQuotes { get; set; } = Array.Empty<LocalAiMarketQuoteContextDTO>();
        public IReadOnlyList<LocalAiMarketSnapshotContextDTO> MarketSnapshots { get; set; } = Array.Empty<LocalAiMarketSnapshotContextDTO>();
        public IReadOnlyList<LocalAiMarketTradeContextDTO> MarketTrades { get; set; } = Array.Empty<LocalAiMarketTradeContextDTO>();

        // =====================================================
        // PROVIDERS
        // =====================================================

        public IReadOnlyList<LocalAiProviderContextDTO> Providers { get; set; } = Array.Empty<LocalAiProviderContextDTO>();
        public IReadOnlyList<LocalAiProviderInstrumentContextDTO> ProviderInstruments { get; set; } = Array.Empty<LocalAiProviderInstrumentContextDTO>();

        // =====================================================
        // TECHNICAL ANALYSIS
        // =====================================================

        public IReadOnlyList<LocalAiTechnicalIndicatorContextDTO> TechnicalIndicators { get; set; } = Array.Empty<LocalAiTechnicalIndicatorContextDTO>();
        // =====================================================
        // USER CONTEXT
        // =====================================================
        public IReadOnlyList<LocalAiUserMessageContextDTO> UserMessages { get; set; } = Array.Empty<LocalAiUserMessageContextDTO>();
    }
}










































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.