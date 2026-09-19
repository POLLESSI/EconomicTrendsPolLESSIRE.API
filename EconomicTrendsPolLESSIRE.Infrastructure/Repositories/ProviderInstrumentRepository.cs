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

        public async Task<int> ArchivePastProviderInstrumentsAsync(CancellationToken ct = default)
        {
            const string sql = @"
                            UPDATE [ProviderInstrument]
                                    SET [Active] = 0
                                    WHERE [Active] = 1
                            ";

            try
            {
                var affectedRows = await _connection.ExecuteAsync(sql);
                _logger.LogInformation("{Count} provider instrument(s) archived.", affectedRows);
                return affectedRows;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error archiving past provider instruments");
                return 0;
            }
        }

        public async Task<bool> DeleteProviderInstrumentAsync(int id)
        {
            const string sql = @"
                            DELETE FROM Instrument WHERE ProviderId = @ProviderId
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProviderId", id, DbType.Int64);

                var affectedRows = await _connection.ExecuteAsync(sql, parameters);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error archiving past provider instruments");
                return false;
            }
        }

        public Task<IEnumerable<ProviderInstrument>> GetAllProviderInstrumentAsync(int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(@Limit) [ProviderId], [InstrumentId], [ProviderSymbol], [Realtime], [DelaySeconds], [Active]
                            FROM [ProviderInstrument]
                            WHERE [Active] = 1
                            ORDER BY [CreatedAtUtc] ASC; 
                            ";

            try
            {
                return _connection.QueryAsync<ProviderInstrument>(new CommandDefinition(sql, new { Limit = limit }, cancellationToken: ct));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geting all past provider instruments");
                return null;
            }
        }

        public async Task<ProviderInstrument?> GetProviderInstrumentByIdAsync(int id)
        {
            const string sql = @"
                            SELECT TOP(1) [ProviderId], [InstrumentId], [ProviderSymbol], [Realtime], [DelaySeconds], [Active]
                            FROM dbo.ProviderInstrument
                            WHERE ProviderId = @ProviderId
                              AND Active = 1;
                            ";

            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ProviderId", id, DbType.Int64);

                var cmd = new CommandDefinition(sql, parameters);

                return await _connection.QueryFirstOrDefaultAsync<ProviderInstrument>(cmd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Provider Instrument by Id={ProviderId}", id);
                return null;
            }
        }

        public async Task<ProviderInstrument?> SaveProviderInstrumentAsync(ProviderInstrument providinstrument)
        {
            const string sql = @"
                            INSERT INTO [ProviderInstrument] ([InstrumentId], [ProviderSymbol], [Realtime], [DelaySeconds], [Active])
                            VALUES (@InstrumentId, @ProviderSymbol, @Realtime, @DelaySeconds, 1);
                            SELECT CAST(SCOPE_IDENTITY() as BIGINT);"";
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Symbol", providinstrument.InstrumentId, DbType.UInt64);
                parameters.Add("@Name", providinstrument.ProviderSymbol, DbType.String);
                parameters.Add("@AssetClass", providinstrument.Realtime, DbType.Boolean);
                parameters.Add("@ExchangeCode", providinstrument.DelaySeconds, DbType.Int16);

                var newId = await _connection.ExecuteScalarAsync<int>(sql, parameters);
                providinstrument.ProviderId = newId;
                providinstrument.Active = true;
                return providinstrument;
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                _logger.LogWarning(sqlEx, "Duplicate active provider instrument {ProviderSymbol} on exchange {Realtime}", providinstrument.ProviderSymbol, providinstrument.Realtime);

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Provider Instrument");
                return null;
            }
        }
    }
}












































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.