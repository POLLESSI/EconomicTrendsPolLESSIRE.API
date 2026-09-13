using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMessageTriageService
    {
        Task<MessageTriageResult> AnalyzeAsync(UserMessage message, CancellationToken ct = default);
    }
}
