namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMistralRequestRegistry
    {
        string Register(int interactionId, CancellationTokenSource cts);

        bool TryGet(int interactionId, out string requestId, out CancellationTokenSource? cts);

        bool TryCancel(int interactionId, string? requestId = null);

        void Remove(int interactionId, string? requestId = null);
    }
}
