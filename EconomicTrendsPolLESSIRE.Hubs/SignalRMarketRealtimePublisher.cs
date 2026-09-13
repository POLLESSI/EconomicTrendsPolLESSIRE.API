using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace EconomicTrendsPolLESSIRE.Hubs
{
    public sealed class SignalRMarketRealtimePublisher : IMarketRealtimePublisher
    {
        private readonly IHubContext<MarketDataHub> _hubContext;
        public SignalRMarketRealtimePublisher( IHubContext<MarketDataHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task PublishAsync<T>(string eventName, T payload, CancellationToken ct = default)
        {
            return _hubContext.Clients.All.SendAsync(eventName, payload, ct);
        }
    }
}