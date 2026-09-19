using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Queries;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IUserSessionsRepository
    {
        Task UpsertAsync(UserSession s);
        Task TouchAsync(string jti, DateTime nowUtc);
        Task<bool> IsRevokedAsync(string jti);
        Task<int> RevokeAsync(string jti, string reason);
        Task<IEnumerable<UserSession>> QueryAsync(SessionQuery q);
        Task<int> PurgeExpiredAsync();
    }
}



































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.