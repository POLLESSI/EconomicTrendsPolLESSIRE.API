using EconomicTrendsPolLESSIRE.Application.Mistral;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMistralBackgroundQueue
    {
        ValueTask QueueAsync(MistralWorkItem workItem, CancellationToken cancellationToken = default);
        ValueTask<MistralWorkItem> DequeueAsync(CancellationToken cancellationToken = default);
    }
}











































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.