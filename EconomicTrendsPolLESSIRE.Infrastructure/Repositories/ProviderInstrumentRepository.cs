using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class ProviderInstrumentRepository : IProviderInstrumentRepository
    {
    #nullable disable
        private readonly System.Data.IDbConnection _connection;
        private readonly ILogger<ProviderInstrumentRepository> _logger;

        public ProviderInstrumentRepository(IDbConnection connection, ILogger<ProviderInstrumentRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public Task<int> ArchivePastProviderInstrumentsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProviderInstrumentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProviderInstrument>> GetAllProviderInstrumentAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrument?> GetProviderInstrumentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderInstrument?> SaveProviderInstrumentAsync(ProviderInstrument Providinstrument)
        {
            throw new NotImplementedException();
        }
    }
}
