using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Enums;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IUserMessageAdminQueueRepository
    {
        Task<int> CreateAsync(UserMessageAdminQueue item, CancellationToken ct = default);
        Task<IReadOnlyList<AdminMessageQueueDto>> GetAsync(AdminMessageQueueFilter filter, CancellationToken ct = default);
        Task<bool> UpdateStatusAsync(int id, AdminMessageStatus status, string? adminNote, string? assignedTo, CancellationToken ct = default);
    }
}
