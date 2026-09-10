using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IInstrumentService
    {
        Task<InstrumentDTO?> GetByIdAsync(int id);
        Task<InstrumentDTO?> SaveAsync(InstrumentDTO dto);
        Task<Instrument?> SaveInstrumentAsync(Instrument instrument, CancellationToken ct = default);
        Task<IEnumerable<Instrument>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<Instrument?> GetInstrumentByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteInstrumentAsync(int id, CancellationToken ct = default);
    }
}
