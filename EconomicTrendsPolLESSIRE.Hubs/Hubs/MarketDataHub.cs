using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Hubs.Hubs
{
    public sealed class MarketDataHub : Hub
    {
    #nullable disable

        private readonly ILogger<MarketDataHub> _logger;

        public MarketDataHub(ILogger<MarketDataHub> logger)
        {
            _logger = logger;
        }

        public async Task RefreshMarketData(string message)
        {
            _logger.LogInformation("RefreshMarketData: {Message}", message);

            await Clients.All.SendAsync(MarketHubMethods.ToClient.MarketDataRefreshed, message);
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User?.IsInRole("Admin") == true || Context.User?.IsInRole("Police") == true)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, MarketHubMethods.AuthorizedGroup);
            }

            await base.OnConnectedAsync();
        }
    }
}

























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.