using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IMistralInteractionRepository
    {
        Task<MistralInteraction?> UpsertInteractionAsync(MistralInteraction interaction);
        Task<MistralInteraction> CreatePendingAsync(MistralInteraction interaction, CancellationToken ct = default);
        Task SaveInteractionAsync(string prompt, string response, DateTime timestamp);
        Task SaveInteractionAsync(MistralInteraction interaction);
        Task<IEnumerable<MistralInteraction>> GetAllInteractionsAsync();
        Task<MistralInteraction?> GetByIdAsync(int id);
        Task<bool> DeactivateInteractionAsync(int id);
        Task<int> ArchivePastMistralInteractionsAsync();
        Task<bool> UpdateResponseAsync(int interactionId, string response, CancellationToken ct = default);
        Task<bool> UpdateLocationAsync(int interactionId, double latitude, double longitude, CancellationToken ct = default);
        Task<bool> MarkFailedAsync(int interactionId, string? errorMessage, CancellationToken ct = default);
        Task<bool> MarkCancelledAsync(int interactionId, string? message = null, CancellationToken ct = default);
        Task<bool> CompleteAsync(int interactionId, string response, string sourceType, CancellationToken ct = default);
    }
}
