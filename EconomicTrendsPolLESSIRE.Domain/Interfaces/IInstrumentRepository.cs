using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IInstrumentRepository
    {
        Task<Instrument?> SaveInstrumentAsync(Instrument instrument);
        Task<IEnumerable<Instrument>> GetAllInstrumentAsync(int limit = 200, CancellationToken ct = default);
        Task<Instrument?> GetInstrumentByIdAsync(int id);
        Task<bool> DeleteInstrumentAsync(int id);
        Task<int> ArchivePastInstrumentsAsync(CancellationToken ct = default);
    }
}
