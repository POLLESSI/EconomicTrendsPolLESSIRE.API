using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class InstrumentService : IInstrumentService
    {
        public Task<bool> DeleteInstrumentAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Instrument>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<InstrumentDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Instrument?> GetInstrumentByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<InstrumentDTO?> SaveAsync(InstrumentDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<Instrument?> SaveInstrumentAsync(Instrument instrument, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
