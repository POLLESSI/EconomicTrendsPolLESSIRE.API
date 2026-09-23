using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Hubs.Extensions
{
    public static class MistralHubContextExtensions
    {
        public static async Task SendStarted(this IHubContext<MistralHub, IMistralClient> hubContext, MistralResponseStartedDto dto, ILogger? logger = null)
        {
            logger?.LogInformation("[Mistral-HUB] SendStarted -> InteractionId={InteractionId}, RequestId={RequestId}", dto.InteractionId, dto.RequestId);

            await hubContext.Clients.All.ReceiveMistralResponseStarted(dto);
        }

        public static async Task SendChunk(this IHubContext<MistralHub, IMistralClient> hubContext, MistralResponseChunkDto dto, ILogger? logger = null)
        {
            logger?.LogInformation(
                "[Mistral-HUB] SendChunk -> InteractionId={InteractionId}, RequestId={RequestId}, ChunkLength={ChunkLength}, IsFinal={IsFinal}",
                dto.InteractionId,
                dto.RequestId,
                dto.Chunk?.Length ?? 0,
                dto.IsFinal);

            await hubContext.Clients.All.ReceiveMistralResponseChunk(dto);

            logger?.LogInformation("[Mistral STREAM] chunk {Length}", dto.Chunk?.Length ?? 0);
        }

        public static async Task SendStatus(this IHubContext<MistralHub, IMistralClient> hubContext, MistralResponseStatusDto dto, ILogger? logger = null)
        {
            logger?.LogInformation("[Mistral-HUB] SendStatus -> InteractionId={InteractionId}, RequestId={RequestId}, Status={Status}", dto.InteractionId, dto.RequestId, dto.Status);

            await hubContext.Clients.All.ReceiveMistralResponseStatus(dto);
        }

        public static async Task SendCompleted(this IHubContext<MistralHub, IMistralClient> hubContext, MistralInteractionCompletedDto dto, ILogger? logger = null)
        {
            logger?.LogInformation("[Mistral-HUB] SendCompleted -> InteractionId={InteractionId}", dto.Id);

            await hubContext.Clients.All.ReceiveMistralResponseCompleted(dto);

            logger?.LogInformation("[Mistral STREAM] completed");
        }
    }
}



















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.