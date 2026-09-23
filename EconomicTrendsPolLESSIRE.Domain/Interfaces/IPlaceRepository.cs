using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IPlaceRepository
    {
        Task<IEnumerable<Place?>> GetLatestPlaceAsync(int limit = 200, CancellationToken ct = default);

        Task<IEnumerable<Place>> GetNearbyPlacesAsync(
            double latitude,
            double longitude,
            double radiusKm,
            CancellationToken ct = default);

        Task<Place?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<Place> SavePlaceAsync(Place place, CancellationToken ct = default);
        Task<IReadOnlyList<Place>> GetByNameAsync(string name, CancellationToken ct = default);
        Task<IReadOnlyList<Place>> GetByTypeAsync(string type, CancellationToken ct = default);
        Place? UpdatePlace(Place place);

        Task<Place?> UpdateAsync(Place place, CancellationToken ct = default);
        Task<Place?> FindByNameLikeAsync(string keyword, CancellationToken ct = default);
        Task<IReadOnlyList<Place>> GetActivePlacesAsync(CancellationToken ct = default);
    }
}
