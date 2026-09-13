using EconomicTrendsPolLESSIRE.Domain.Common;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IProfanityAdminService
    {
        Task<PagedResultDto<ProfanityWord>> GetPagedAsync(int page, int pageSize, string? languageCode = null, string? search = null, CancellationToken ct = default);
        Task<ProfanityWord?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ProfanityWord> CreateAsync(ProfanityWord entity, CancellationToken ct = default);
        Task<bool> UpdateAsync(ProfanityWord entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
        Task<bool> SetActiveAsync(int id, bool active, CancellationToken ct = default);
    }
}
