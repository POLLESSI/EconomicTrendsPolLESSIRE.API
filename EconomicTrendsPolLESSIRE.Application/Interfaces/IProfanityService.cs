using EconomicTrendsPolLESSIRE.Domain.Models;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IProfanityService
    {
        string Normalize(string content);

        Task<bool> ContainsProfanityAsync(string content, CancellationToken ct = default);

        Task<ProfanityAnalysisResult> AnalyzeAsync(string content, CancellationToken ct = default);

        bool ContainsProfanity(string content);

        int GetToxicityScore(string content);

        IReadOnlyCollection<string> GetMatchedWords(string content);

        string Sanitize(string content);
    }
}
















































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.