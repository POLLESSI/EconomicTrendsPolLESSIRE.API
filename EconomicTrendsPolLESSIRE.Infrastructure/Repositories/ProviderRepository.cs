using Dapper;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Microsoft.Data.SqlClient;
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

        public async Task<int> ArchivePastProvidersAsync(CancellationToken ct = default)
        {
            const string sql = @"
                            UPDATE [Provider]
                                    SET [Active] = 0
                                    WHERE [Active] = 1
                                        AND [CreatedAtUtc] < DATEADD(DAY, -1, CAST(GETDATE() AS DATETIME2(0)));"";
                            ";

            try
            {
                var affectedRows = await _connection.ExecuteAsync(sql);
                _logger.LogInformation("{Count} provider(s) archived.", affectedRows);
                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error archiving past providers");
                return 0;
            }
        }

        public async Task<bool> DeleteProviderAsync(int id)
        {
            const string sql = @"
                            DELETE FROM Provider WHERE Id = @Id
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int64);

                var affectedRows = await _connection.ExecuteAsync(sql, parameters);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting past providers");
                return false;
            }
        }

        public Task<IEnumerable<Provider>> GetAllProviderAsync(int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(@Limit) [Id], [Code], [Name], [CreatedAtUtc], [Active]
                            FROM [Provider]
                            WHERE [Active] = 1
                            ORDER BY [CreatedAtUtc] ASC;
                            ";

            try
            {
                return _connection.QueryAsync<Provider>(new CommandDefinition(sql, new { Limit = limit }, cancellationToken: ct));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geting all past providers");
                return null;
            }
        }

        public async Task<Provider?> GetProviderByIdAsync(int id, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(1) [Id], [Code], [Name], [CreatedAtUtc], [Active]
                            FROM dbo.Provider
                            WHERE Id = @Id
                              AND Active = 1;
                            ";

            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", id, DbType.Int64);

                var cmd = new CommandDefinition(sql, parameters);

                return await _connection.QueryFirstOrDefaultAsync<Provider>(cmd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting provider by Id={Id}", id);
                return null;
            }
        }

        public async Task<Provider?> SaveProviderAsync(Provider provider)
        {
            const string sql = @"
                            INSERT INTO [Provider] ([Code], [Name], [CreatedAtUtc], [Active])
                            VALUES (@Code, @Name, @CreatedAtUtc, 1);
                            SELECT CAST(SCOPE_IDENTITY() as BIGINT);
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", provider.Code, DbType.String);
                parameters.Add("@Name", provider.Name, DbType.String);
                parameters.Add("@CreatedAtUtc", provider.CreatedAtUtc, DbType.DateTime2);

                var newId = await _connection.ExecuteScalarAsync<int>(sql, parameters);
                provider.Id = newId;
                provider.Active = true;
                return provider;
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                _logger.LogWarning(sqlEx, "Duplicate active instrument {Code} on exchange {CreatedAtUtc}", provider.Code, provider.CreatedAtUtc);

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Instrument");
                return null;
            }
        }
    }
}














































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.