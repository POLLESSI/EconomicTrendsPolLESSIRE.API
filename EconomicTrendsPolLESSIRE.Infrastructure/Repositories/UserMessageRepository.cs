using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class UserMessageRepository : IUserMessageRepository
    {
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
