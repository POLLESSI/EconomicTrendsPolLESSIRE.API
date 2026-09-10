using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMarkerSnapshotService
    {
        Task<MarkerSnapshotDTO?> GetByIdAsync(int id);
        Task<MarkerSnapshotDTO?> SaveAsync(MarkerSnapshotDTO dto);
        Task<MarkerSnapshot?> SaveMarkerSnapshotAsync(MarkerSnapshot markersnsht, CancellationToken ct = default);
        Task<IEnumerable<MarkerSnapshot>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<MarkerSnapshot?> GetMarkerSnapshotByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteMarkerSnapshotAsync(int id, CancellationToken ct = default);
    }
}
