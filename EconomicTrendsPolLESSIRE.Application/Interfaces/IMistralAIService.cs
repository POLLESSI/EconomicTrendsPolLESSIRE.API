using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMistralAIService
    {
        Task<IEnumerable<Suggestion>> GetWeatherAdvisoryAsync(string location, CancellationToken ct = default);

        Task<string> CallOllamaApi(string prompt, CancellationToken ct);

        Task<int> ArchivePastGptInteractionsAsync();

        Task<string> GenerateFromPromptAsync(string groundedPrompt, string responseLanguage = "fr-FR", CancellationToken ct = default);

        Task<string> StreamFromPromptAsync(string groundedPrompt, Func<string, Task> onChunk, string responseLanguage = "fr-FR", CancellationToken ct = default);
    }
}

















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.