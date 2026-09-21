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

        public async Task<bool> DeleteProviderInstrumentAsync(int providerId, long instrumentId)
        {
            const string sql = @"
                            DELETE FROM dbo.ProviderInstrument
                            WHERE ProviderId = @ProviderId
                              AND InstrumentId = @InstrumentId;
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProviderId", providerId, DbType.Int16);
                parameters.Add("@InstrumentId", instrumentId, DbType.Int64);

                var affectedRows = await _connection.ExecuteAsync(sql, parameters);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error archiving ProviderInstrument {ProviderId}/{InstrumentId}", providerId, instrumentId);
                return false;
            }
        }

        public Task<IEnumerable<ProviderInstrument>> GetAllProviderInstrumentAsync(int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(@Limit) [ProviderId], [InstrumentId], [ProviderSymbol], [Realtime], [DelaySeconds], [Active]
                            FROM [ProviderInstrument]
                            WHERE [Active] = 1
                            ORDER BY ProviderId, InstrumentId; 
                            ";

            try
            {
                var cmd = new CommandDefinition(sql, new { Limit = limit }, cancellationToken: ct);
                return _connection.QueryAsync<ProviderInstrument>(cmd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geting all past provider instruments");
                return null;
            }
        }

        public async Task<ProviderInstrument?> GetProviderInstrumentByIdAsync(int providerId, long instrumentId, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(1) [ProviderId], [InstrumentId], [ProviderSymbol], [Realtime], [DelaySeconds], [Active]
                            FROM dbo.ProviderInstrument
                            WHERE ProviderId = @ProviderId
                              AND InstrumentId = @InstrumentId
                              AND Active = 1;
                            ";

            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ProviderId", providerId, DbType.Int16);
                parameters.Add("@InstrumentId", instrumentId, DbType.Int64);

                var cmd = new CommandDefinition(sql, parameters, cancellationToken: ct);

                return await _connection.QueryFirstOrDefaultAsync<ProviderInstrument>(cmd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ProviderInstrument {ProviderId}/{InstrumentId}", providerId, instrumentId);
                return null;
            }
        }

        public async Task<ProviderInstrument?> SaveProviderInstrumentAsync(ProviderInstrument providinstrument)
        {
            const string sql = @"
                            INSERT INTO [ProviderInstrument] ([ProviderId], [InstrumentId], [ProviderSymbol], [Realtime], [DelaySeconds], [Active])
                            VALUES (@ProviderId, @InstrumentId, @ProviderSymbol, @Realtime, @DelaySeconds, 1);
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProviderId", providinstrument.ProviderId, DbType.Int16);
                parameters.Add("@InstrumentId", providinstrument.InstrumentId, DbType.UInt64);
                parameters.Add("@ProviderSymbol", providinstrument.ProviderSymbol, DbType.String);
                parameters.Add("@Realtime", providinstrument.Realtime, DbType.Boolean);
                parameters.Add("@DelaySeconds", providinstrument.DelaySeconds, DbType.Int16);

                var affected = await _connection.ExecuteAsync(sql, parameters);
                if (affected != 1) { return null; }

                providinstrument.Active = true;
                return providinstrument;
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                _logger.LogWarning(sqlEx, "Duplicate active ProviderInstrument {ProviderId}/{InstrumentId} ({ProviderSymbol})", providinstrument.ProviderId, providinstrument.InstrumentId, providinstrument.ProviderSymbol);

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving ProviderInstrument {ProviderId}/{InstrumentId}", providinstrument.ProviderId, providinstrument.InstrumentId);
                return null;
            }
        }
    }
}












































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.