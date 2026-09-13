using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Dapper;
using System.Data;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class UserMessageRepository : IUserMessageRepository
    {
        private readonly IDbConnection _db;

        public UserMessageRepository(IDbConnection db)
        {
            _db = db;
        }
        public Task<int> ArchivePastUserMessagesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserMessageAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserMessage>> GetAllUserMessageAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserMessage>> GetLatestAsync(int take = 100, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserMessage?> GetUserMessageByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserMessage> InsertAsync(UserMessage msg, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
