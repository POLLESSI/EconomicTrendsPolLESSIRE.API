using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using Instrument = EconomicTrendsPolLESSIRE.Domain.Entities.Instrument;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class InstrumentRepository : IInstrumentRepository
    {
    #nullable disable
        private readonly IDbConnection _connection;
        private readonly ILogger<InstrumentRepository> _logger;

        public InstrumentRepository(IDbConnection connection, ILogger<InstrumentRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

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
