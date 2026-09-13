using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMessageCorrelationService
    {
        Task<UserMessage> CorrelateAsync(UserMessage raw, CancellationToken ct = default);
    }
}
