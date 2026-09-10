using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class InstrumentRepository : IInstrumentRepository
    {
        public Task<int> ArchivePastInstrumentsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteInstrumentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Instrument>> GetAllInstrumentAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Instrument?> GetInstrumentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Instrument?> SaveInstrumentAsync(Instrument instrument)
        {
            throw new NotImplementedException();
        }
    }
}
