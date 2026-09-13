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
    public class TechnicalIndicatorRepository : ITechnicalIndicatorRepository
    {
    #nullable disable
        private readonly System.Data.IDbConnection _connection;
        private readonly ILogger<TechnicalIndicatorRepository> _logger;

        public TechnicalIndicatorRepository(IDbConnection connection, ILogger<TechnicalIndicatorRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public Task<int> ArchivePastTechnicalIndicatorsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTechnicalIndicatorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TechnicalIndicator>> GetAllTechnicalIndicatorAsync(int limit = 200, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicator?> GetTechnicalIndicatorByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TechnicalIndicator?> SaveTechnicalIndicatorAsync(TechnicalIndicator technicalindic)
        {
            throw new NotImplementedException();
        }
    }
}
