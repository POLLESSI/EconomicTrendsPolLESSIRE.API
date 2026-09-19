using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface IInstrumentRepository
    {
        Task<Instrument?> SaveInstrumentAsync(Instrument instrument);
        Task<IEnumerable<Instrument>> GetAllInstrumentAsync(int limit = 200, CancellationToken ct = default);
        Task<Instrument?> GetInstrumentByIdAsync(long id, CancellationToken ct = default);
        Task<bool> DeleteInstrumentAsync(long id);
        Task<int> ArchivePastInstrumentsAsync(CancellationToken ct = default);
    }
}





























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.