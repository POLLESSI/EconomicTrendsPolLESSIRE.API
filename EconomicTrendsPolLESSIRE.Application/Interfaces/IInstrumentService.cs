using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IInstrumentService
    {
        Task<InstrumentDTO?> GetByIdAsync(int id);
        Task<InstrumentDTO?> SaveAsync(InstrumentDTO dto);
        Task<EconomicTrendsPolLESSIRE.Domain.Entities.Instrument?> SaveInstrumentAsync(EconomicTrendsPolLESSIRE.Domain.Entities.Instrument instrument, CancellationToken ct = default);
        Task<IEnumerable<EconomicTrendsPolLESSIRE.Domain.Entities.Instrument>> GetAllAsync(int limit = 500, CancellationToken ct = default);
        Task<EconomicTrendsPolLESSIRE.Domain.Entities.Instrument?> GetInstrumentByIdAsync(int id, CancellationToken ct = default);
        Task<bool> DeleteInstrumentAsync(int id, CancellationToken ct = default);
    }
}
