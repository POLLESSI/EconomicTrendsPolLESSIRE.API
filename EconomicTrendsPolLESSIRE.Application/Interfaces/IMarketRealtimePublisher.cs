using EconomicTrendsPolLESSIRE.Contracts.DTOs;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMarketRealtimePublisher
    {
        Task PublishAsync<T>(string eventName, T payload, CancellationToken ct = default);
        //Task PublishProviderUpdatedAsync(ProviderDTO provider, CancellationToken ct = default);
        //Task PublishTradeAsync(MarketTradeDTO trade, CancellationToken ct = default);
        //Task PublishQuoteAsync(MarketQuoteDTO quote, CancellationToken ct = default);
        //Task PublishCandleAsync(MarketCandleDTO candle, CancellationToken ct = default);
    }
}
