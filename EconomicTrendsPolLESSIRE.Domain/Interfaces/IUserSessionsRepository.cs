using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Queries;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IUserSessionsRepository
    {
        Task UpsertAsync(UserSessions s);
        Task TouchAsync(string jti, DateTime nowUtc);
        Task<bool> IsRevokedAsync(string jti);
        Task<int> RevokeAsync(string jti, string reason);
        Task<IEnumerable<UserSessions>> QueryAsync(SessionQuery q);
        Task<int> PurgeExpiredAsync();
    }
}
