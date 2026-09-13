using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace EconomicTrendsPolLESSIRE.Hubs.Hubs
{
    public sealed class UserHub : Hub
    {
        public async Task NotifyUserRegistered(string email)
        {
            await Clients.All.SendAsync(
                UserHubMethods.ToClient.UserRegistered,
                email);
        }
    }
}
