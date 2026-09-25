namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public class MistralInteraction
    {
    #nullable disable
        // ID of the AI ​​interaction itself
        public int Id { get; set; }
        public string Prompt { get; set; }
        public string PromptHash { get; set; }
        public string Response { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; } = true;

        public string Model { get; set; }
        public float? Temperature { get; set; }
        public int? TokenCount { get; set; }

        // =====================================================
        // OPTIONAL MARKET CONTEXT
        // =====================================================

        // Instrument.Id = BIGINT
        // Also used by MarketSnapshot,
        // MarketCandle, ProviderInstrument and TechnicalIndicator
        public long? InstrumentId { get; set; }

        // MarketQuote.Id = BIGINT
        public long? MarketQuoteId { get; set; }

        // MarketTrade.Id = BIGINT
        public long? MarketTradeId { get; set; }

        // Provider.Id = SMALLINT in SQL,
        // but your C# domain currently uses int
        public int? ProviderId { get; set; }

        // MarketCandle composite key:
        // InstrumentId + IntervalCode + OpenTimeUtc
        public int? MarketCandleIntervalCode { get; set; }

        public DateTime? MarketCandleOpenTimeUtc { get; set; }

        // TechnicalIndicator composite key:
        // InstrumentId + IntervalCode +
        // IndicatorType + TimestampUtc
        public int? TechnicalIndicatorIntervalCode { get; set; }

        public int? TechnicalIndicatorType { get; set; }

        public DateTime? TechnicalIndicatorTimestampUtc { get; set; }



        // =====================================================
        // LOCATION SNAPSHOT
        // =====================================================

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        // Instrument
        // MarketCandle
        // MarketQuote
        // MarketSnapshot
        // MarketTrade
        // Provider
        // ProviderInstrument
        // TechnicalIndicator
        public string? SourceType { get; set; }

        public string ExecutionSource { get; set; } = "MistralLocal";

        public string Status { get; set; } = "Pending";

        public DateTime? DateDeleted { get; set; }
    }
}






































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.