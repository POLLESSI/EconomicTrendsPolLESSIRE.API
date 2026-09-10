using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class UserMessageService : IUserMessageService
    {
        public Task<int> ArchivePastUserMessagesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserMessageAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserMessage>> GetLatestAsync(int take = 100, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserMessage?> GetUserMessageByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserMessage> InsertAsync(UserMessage msg, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserMessage> InsertAsync(UserMessage msg, bool requestAdminReview, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
