using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Hubs.Hubs
{
    [Authorize]
    public sealed class MistralHub : Hub<IMistralClient>
    {
        private readonly ILogger<MistralHub> _logger;

        public MistralHub(ILogger<MistralHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("[GPTHub] Connected: {ConnectionId}", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception? exception)
        {
            _logger.LogInformation(
                "[GPTHub] Disconnected: {ConnectionId}. Reason={Reason}",
                Context.ConnectionId,
                exception?.Message);

            await base.OnDisconnectedAsync(exception);
        }

        // Legacy only if you still need it.
        public Task RefreshGpt(string message)
        {
            _logger.LogInformation("[GPTHub] RefreshGpt called. MessageLength={Length}", message?.Length ?? 0);
            return Task.CompletedTask;
        }
    }
}



















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.