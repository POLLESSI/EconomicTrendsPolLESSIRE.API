using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMistralOrchestrator
    {
        Task<MistralStartResponseDto> StartMistralRequestAsync(MistralPromptRequest request, CancellationToken ct = default);

        Task<MistralInteractionDTO> RunMistralRequestAsync(MistralPromptRequest request, CancellationToken ct = default);

        Task<bool> CancelAsync(int interactionId, string? requestId = null);
    }
}
