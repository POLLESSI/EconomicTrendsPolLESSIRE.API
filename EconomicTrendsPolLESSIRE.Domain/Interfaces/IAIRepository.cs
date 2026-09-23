using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IAIRepository
    {
        Task<string> GetSuggestionsAsync(object content);
        /// <summary>
        /// Sends a structured prompt to the AI ​​(OpenAI) and returns the generated response.
        /// </summary>
        /// <param name="prompt">The structured prompt containing the tourist constraints.</param>
        /// <returns>AI-generated text response.</returns>
        Task<string> GetEconomicTrendSuggestionsAsync(string prompt);
        Task<MistralInteraction?> GetChatMistralByIdAsync(int id);
        Task<string> SummarizeTextAsync(string input);
        Task<string> GenerateSuggestionAsync(string prompt);
        Task<string> AskChatMistralAsync(string prompt);
        Task<string> TranslateToFrenchAsync(string englishText);
        Task<string> TranslateToDutchAsync(string englishText);
        Task<string> TranslateToGermanAsync(string englishText);
        Task<string> SuggestAlternativeAsync(string prompt);
        Task<string> SuggestAlternativeWithWeatherAsync(string location);
        Task SaveInteractionAsync(MistralInteraction interaction);
        Task<MistralInteraction?> GetByIdAsync(int id);
    }
}
