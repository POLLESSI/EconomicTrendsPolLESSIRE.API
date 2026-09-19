using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IUserHubService
    {
        Task BroadcastUserUpdatedAsync(CancellationToken cancellationToken = default);
        Task NotifyUserRegistered(string email); // existante ?
        Task NotifyUserUpdated(Users user);
        Task NotifyUserDeactivated(int id);
    }
}


































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.