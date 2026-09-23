using EconomicTrendsPolLESSIRE.Domain.DTOs;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface ILocalAiContextService
    {
        Task<LocalAiContextDTO> BuildContextAsync(string prompt, double? latitude, double? longitude, CancellationToken ct = default);

        string BuildPrompt(LocalAiContextDTO context);
    }
}
















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.