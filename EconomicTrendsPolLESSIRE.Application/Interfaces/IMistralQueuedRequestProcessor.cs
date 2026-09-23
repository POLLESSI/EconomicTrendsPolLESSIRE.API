using EconomicTrendsPolLESSIRE.Application.Mistral;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMistralQueuedRequestProcessor
    {
        Task ProcessQueuedAsync(MistralWorkItem workItem, CancellationToken stoppingToken);
    }
}
