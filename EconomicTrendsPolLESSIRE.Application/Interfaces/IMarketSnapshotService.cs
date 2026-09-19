using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IMarketSnapshotService
    {
        Task<MarketSnapshotDTO?> GetByIdAsync(int id);
        Task<MarketSnapshotDTO?> SaveAsync(MarketSnapshotDTO dto);
        Task<MarketSnapshot?> SaveMarketSnapshotAsync(MarketSnapshot marketsnsht, CancellationToken ct = default);
        Task<IEnumerable<MarketSnapshot>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<MarketSnapshot?> GetMarketSnapshotByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteMarketSnapshotAsync(int id, CancellationToken ct = default);
    }
}























































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.