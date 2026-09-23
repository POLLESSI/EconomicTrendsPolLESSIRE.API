using System.Threading;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMistralExternalService
    {
        Task<string> CompleteAsync(string prompt, CancellationToken ct = default);
        Task<string> RefineSuggestionAsync(string raw, CancellationToken ct = default);
    }
}
