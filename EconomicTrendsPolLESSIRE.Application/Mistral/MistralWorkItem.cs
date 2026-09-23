using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Mistral
{
    public sealed record MistralWorkItem(MistralInteraction Interaction, MistralPromptRequest Request, string RequestId, CancellationToken ProcessingToken);
    
}
