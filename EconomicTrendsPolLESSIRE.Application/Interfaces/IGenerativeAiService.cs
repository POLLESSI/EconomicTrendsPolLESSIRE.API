namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IGenerativeAiService
    {
        Task<string> GenerateTextAsync(string prompt, CancellationToken ct = default);
    }
}
