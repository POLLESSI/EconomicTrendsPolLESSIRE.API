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
    public class ProviderRepository : IProviderRepository
    {
    #nullable disable
        private readonly System.Data.IDbConnection _connection;
        private readonly ILogger<ProviderRepository> _logger;

        public ProviderRepository(IDbConnection connection, ILogger<ProviderRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public Task<int> ArchivePastProvidersAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProviderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Provider>> GetAllProviderAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Provider?> GetProviderByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Provider?> SaveProviderAsync(Provider provider)
        {
            throw new NotImplementedException();
        }
    }
}
