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
