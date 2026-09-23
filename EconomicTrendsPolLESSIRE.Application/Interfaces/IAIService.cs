using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IAIService
    {
        Task<string> GetSuggestionsAsync(object content);

        /// <summary>
        /// Sends a structured prompt to the AI ​​(OpenAI) and returns the generated response.
        /// </summary>
        /// <param name="prompt">The structured prompt containing the tourist constraints.</param>
        /// <returns>AI-generated text response.</returns>
        Task<string> GetTouristicSuggestionsAsync(string prompt);
        Task<MistralInteraction?> GetChatGptByIdAsync(int id);
        Task<string> SummarizeTextAsync(string input);
        Task<string> GenerateSuggestionAsync(string prompt);
        Task<string> AskChatGptAsync(string prompt);
        Task<string> TranslateToFrenchAsync(string englishText);
        Task<string> TranslateToDutchAsync(string englishText);
        Task<string> TranslateToGermanAsync(string englishText);
        Task<string> SuggestAlternativeAsync(string prompt);
        Task<string> SuggestAlternativeWithWeatherAsync(string location);
        Task SaveInteractionAsync(string prompt, string reply, DateTime createdAt, CancellationToken ct = default);
        Task<MistralInteraction?> GetByIdAsync(int id);
    }
}
