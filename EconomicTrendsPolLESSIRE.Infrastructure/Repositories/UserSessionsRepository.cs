using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Domain.Queries;
using System.Data;
using Dapper;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class UserSessionsRepository : IUserSessionsRepository
    {
        private readonly IDbConnection _cn;
        public UserSessionsRepository(IDbConnection cn) => _cn = cn;
        public Task<bool> IsRevokedAsync(string jti)
        {
            throw new NotImplementedException();
        }

        public Task<int> PurgeExpiredAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserSessions>> QueryAsync(SessionQuery q)
        {
            throw new NotImplementedException();
        }

        public Task<int> RevokeAsync(string jti, string reason)
        {
            throw new NotImplementedException();
        }

        public Task TouchAsync(string jti, DateTime nowUtc)
        {
            throw new NotImplementedException();
        }

        public Task UpsertAsync(UserSessions s)
        {
            throw new NotImplementedException();
        }
    }
}
