using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Domain.Common;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public sealed class ProfanityAdminService : IProfanityAdminService
    {
        private readonly IProfanityRepository _repo;
        private readonly IProfanityService _profanityService;

        public ProfanityAdminService(IProfanityRepository repo, IProfanityService profanityService)
        {
            _repo = repo;
            _profanityService = profanityService;
        }

        public async Task<PagedResultDto<ProfanityWord>> GetPagedAsync(int page, int pageSize, string? languageCode = null, string? search = null, CancellationToken ct = default)
        {
            return await _repo.GetPagedAsync(page, pageSize, languageCode, search, ct);
        }

        public Task<ProfanityWord?> GetByIdAsync(int id, CancellationToken ct = default)
            => _repo.GetByIdAsync(id, ct);

        public async Task<ProfanityWord> CreateAsync(ProfanityWord entity, CancellationToken ct = default)
        {
            entity.Word = entity.Word?.Trim() ?? string.Empty;
            entity.NormalizedWord = _profanityService.Normalize(entity.Word);

            if (string.IsNullOrWhiteSpace(entity.Word))
                throw new ArgumentException("Word is required.", nameof(entity.Word));

            return await _repo.InsertAsync(entity, ct);
        }

        public async Task<bool> UpdateAsync(ProfanityWord entity, CancellationToken ct = default)
        {
            entity.Word = entity.Word?.Trim() ?? string.Empty;
            entity.NormalizedWord = _profanityService.Normalize(entity.Word);

            if (string.IsNullOrWhiteSpace(entity.Word))
                throw new ArgumentException("Word is required.", nameof(entity.Word));

            return await _repo.UpdateAsync(entity, ct);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
            => _repo.SoftDeleteAsync(id, ct);

        public Task<bool> SetActiveAsync(int id, bool active, CancellationToken ct = default)
            => _repo.SetActiveAsync(id, active, ct);
    }
}
