using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class MarketSnapshotService : IMarkerSnapshotService
    {
        public Task<bool> DeleteMarkerSnapshotAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarkerSnapshot>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarkerSnapshotDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MarkerSnapshot?> GetMarkerSnapshotByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarkerSnapshotDTO?> SaveAsync(MarkerSnapshotDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<MarkerSnapshot?> SaveMarkerSnapshotAsync(MarkerSnapshot markersnsht, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
