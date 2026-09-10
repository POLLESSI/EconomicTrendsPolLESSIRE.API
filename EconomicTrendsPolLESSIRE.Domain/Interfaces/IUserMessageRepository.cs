using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IUserMessageRepository
    {
        Task<UserMessage> InsertAsync(UserMessage msg, CancellationToken ct = default);
        Task<List<UserMessage>> GetLatestAsync(int take = 100, CancellationToken ct = default);
        Task<IEnumerable<UserMessage>> GetAllUserMessageAsync(int limit = 200, CancellationToken ct = default);
        Task<UserMessage?> GetUserMessageByIdAsync(int id);
        Task<bool> DeleteUserMessageAsync(int id);
        Task<int> ArchivePastUserMessagesAsync(CancellationToken ct = default);
    }
}
