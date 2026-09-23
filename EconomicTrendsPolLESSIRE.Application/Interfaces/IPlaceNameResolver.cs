using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IPlaceNameResolver
    {
        Task<Place?> ResolveAsync(string prompt, string? languageCode, CancellationToken ct = default);
    }
}
