using EconomicTrendsPolLESSIRE.Domain.Entities.Telecom;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces.ExternalProviders
{
    public interface ITelecomProviderClient
    {
        string ProviderName { get; }

        Task<Domain.Entities.Telecom.CitizenHackathon2025.Domain.Entities.Telecom.TelecomAntennaSnapshot> GetAntennaSnapshotAsync(
            CancellationToken ct = default);
    }
}
