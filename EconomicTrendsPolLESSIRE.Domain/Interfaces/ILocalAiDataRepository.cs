using EconomicTrendsPolLESSIRE.Domain.DTOs;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface ILocalAiDataRepository
    {
        Task<IReadOnlyList<LocalAiInstrumentContextDTO>> SearchInstrumentsByKeywordsAsync(string userPrompt, int limit = 10, CancellationToken ct = default);
        Task<IReadOnlyList<LocalAiInstrumentContextDTO>> GetActiveInstrumentsAsync( int limit = 100, CancellationToken ct = default);
        Task<IReadOnlyList<LocalAiMarketCandleContextDTO>> GetMarketCandlesAsync(DateTime dateFrom, DateTime dateToExclusive, int limit = 200, CancellationToken ct = default);
        Task<IReadOnlyList<LocalAiMarketQuoteContextDTO>> GetMarketQuotesAsync(DateTime dateFrom, DateTime dateToExclusive, int limit = 200, CancellationToken ct = default);
        Task<IReadOnlyList<LocalAiMarketSnapshotContextDTO>> GetMarketSnapshotsAsync(int limit = 100, CancellationToken ct = default);
        Task<IReadOnlyList<LocalAiMarketTradeContextDTO>> GetMarketTradesAsync(DateTime dateFrom, DateTime dateToExclusive, int limit = 200, CancellationToken ct = default);
        Task<IReadOnlyList<LocalAiProviderContextDTO>> GetActiveProvidersAsync(int limit = 50, CancellationToken ct = default);
        Task<IReadOnlyList<LocalAiProviderInstrumentContextDTO>> GetActiveProviderInstrumentsAsync(int limit = 200, CancellationToken ct = default);
        Task<IReadOnlyList<LocalAiTechnicalIndicatorContextDTO>> GetTechnicalIndicatorsAsync(DateTime dateFrom, DateTime dateToExclusive, int limit = 200, CancellationToken ct = default);
        Task<IReadOnlyList<LocalAiUserMessageContextDTO>> GetRecentUserMessagesAsync(DateTime sinceUtc, int limit = 10, CancellationToken ct = default);
        Task<IReadOnlyList<LocalAiUserMessageContextDTO>> GetNearbyUserMessagesAsync(double latitude, double longitude, double radiusKm, DateTime sinceUtc, int limit = 10, CancellationToken ct = default);
    }
}













































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.