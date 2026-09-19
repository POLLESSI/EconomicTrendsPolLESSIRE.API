using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IUserMessageService
    {
        Task<UserMessage> InsertAsync(UserMessage msg, CancellationToken ct = default);
        Task<UserMessage> InsertAsync(UserMessage msg, bool requestAdminReview, CancellationToken ct = default);
        Task<List<UserMessage>> GetLatestAsync(int take = 100, CancellationToken ct = default);
        Task<UserMessage?> GetUserMessageByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteUserMessageAsync(int id, CancellationToken ct = default);
        Task<int> ArchivePastUserMessagesAsync(CancellationToken ct = default);
    }
}











































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.