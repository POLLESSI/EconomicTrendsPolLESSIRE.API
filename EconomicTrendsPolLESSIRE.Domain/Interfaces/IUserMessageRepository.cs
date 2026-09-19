using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IUserMessageRepository
    {
        Task<UserMessage> InsertAsync(UserMessage msg, CancellationToken ct = default);
        Task<List<UserMessage>> GetLatestAsync(int take = 100, CancellationToken ct = default);
        Task<UserMessage?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteMessageAsync(int id, CancellationToken ct = default);
    }
}






























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.