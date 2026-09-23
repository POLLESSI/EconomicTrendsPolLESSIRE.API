using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.DTOs;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    /// <summary>
    /// Provides a lightweight, query-oriented local AI data projection layer.
    /// The goal is not to expose rich domain entities, but only the minimal DTOs
    /// needed to build an accurate and bounded AI prompt context.
    /// </summary>
    public interface ILocalAiDataRepository
    {
        Task<IReadOnlyList<LocalAiInstrumentContextDTO>> SearchInstrumentsByKeywordsAsync(string userPrompt, int limit = 10, CancellationToken ct = default);
        Task<IEnumerable<LocalAiInstrumentContextDTO>> GetNearbyInstrumentsAsync(double latitude, double longitude, double radiusKm, CancellationToken ct = default);
        /// <summary>
        /// Returns nearby events relevant to the requested date and radius.
        /// Results should already be filtered geographically and temporally.
        /// </summary>
        Task<IEnumerable<LocalAiMarketCandleContextDTO>> GetNearbyMarketCandlesAsync(double latitude, double longitude, DateTime dateFrom, DateTime dateToExclusive, double radiusKm, CancellationToken ct = default);

        /// <summary>
        /// Returns nearby crowd calendar items relevant to the requested date and radius.
        /// Intended for planned / forecast crowd-sensitive events.
        /// </summary>
        Task<IEnumerable<LocalAiMarketQuoteContextDTO>> GetNearbyMarketQuoteAsync(double latitude, double longitude, DateTime dateFrom, DateTime dateToExclusive, double radiusKm, CancellationToken ct = default);

        /// <summary>
        /// Returns recent observed crowd information around the specified location.
        /// The implementation should interpret targetDate pragmatically, typically
        /// as "around that day" or "recent observations relevant to that day".
        /// </summary>
        Task<IEnumerable<LocalAiMarketSnapshotContextDTO>> GetNearbyMarketSnapshotAsync(double latitude, double longitude, DateTime targetDate, double radiusKm, CancellationToken ct = default);

        /// <summary>
        /// Returns nearby traffic incidents or traffic conditions with practical user impact.
        /// The implementation should filter by geographic radius and temporal relevance.
        /// </summary>
        Task<IEnumerable<LocalAiMarketTradeContextDTO>> GetNearbyMarketTradeContextAsync(double latitude, double longitude, DateTime dateFrom, DateTime dateToExclusive, double radiusKm, CancellationToken ct = default);

        /// <summary>
        /// Returns nearby weather points / forecasts relevant to the requested date and radius.
        /// Only the fields needed for practical outing guidance should be returned.
        /// </summary>
        Task<IEnumerable<LocalAiProviderContextDTO>> GetNearbyProviderAsync(double latitude, double longitude, DateTime dateFrom, DateTime dateToExclusive, double radiusKm, CancellationToken ct = default);
        Task<IEnumerable<LocalAiProviderInstrumentContextDTO>> GetNearbyProviderInstrumentsAsync(double latitude, double longitude, double radiusKm, CancellationToken ct = default);
        Task<IEnumerable<LocalAiTechnicalIndicatorContextDTO>> GetNearbyTechnicalIndicatorsAsync(double latitude, double longitude, double radiusKm, CancellationToken ct = default);
        Task<IEnumerable<LocalAiUserMessageContextDTO>> GetNearbyUserMessagesAsync(double latitude, double longitude, double radiusKm, DateTime sinceUtc, int limit = 10, CancellationToken ct = default);
    }
}
