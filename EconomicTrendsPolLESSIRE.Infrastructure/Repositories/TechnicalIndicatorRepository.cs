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

        public async Task<int> ArchivePastTechnicalIndicatorsAsync(CancellationToken ct = default)
        {
            const string sql = @"
                            UPDATE [TechnicalIndicator]
                                    SET [Active] = 0
                                    WHERE [Active] = 1
                                        AND [TimestampUtc] < DATEADD(DAY, -1, CAST(GETDATE() AS DATETIME2(0)));
                            ";

            try
            {
                var affectedRows = await _connection.ExecuteAsync(sql);
                _logger.LogInformation("{Count} Technical Indicator(s) archived.", affectedRows);
                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error archiving past technical indicators");
                return 0;
            }
        }

        public async Task<bool> DeleteTechnicalIndicatorAsync(int id)
        {
            const string sql = @"
                            DELETE FROM Instrument WHERE InstrumentId = @InstrumentId
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@InstrumentId", id, DbType.Int64);

                var affectedRows = await _connection.ExecuteAsync(sql, parameters);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting past technical indicators");
                return false;
            }
        }

        public Task<IEnumerable<TechnicalIndicator>> GetAllTechnicalIndicatorAsync(int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(@Limit) [InstrumentId], [IntervalCode], [TimestampUtc], [IndicatorType], [Value1], [Value2], [Value3], [ParameterHash], [Active]
                            FROM [TechnicalIndicator]
                            WHERE [Active] = 1
                            ORDER BY [TimestampUtc] ASC;
                            ";

            try
            {
                return _connection.QueryAsync<TechnicalIndicator>(new CommandDefinition(sql, new { Limit = limit }, cancellationToken: ct));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geting all technical indicators");
                return null;
            }
        }

        public async Task<TechnicalIndicator?> GetTechnicalIndicatorByIdAsync(int id)
        {
            const string sql = @"
                            SELECT TOP(1) [InstrumentId], [IntervalCode], [TimestampUtc], [IndicatorType], [Value1], [Value2], [Value3], [ParameterHash], [Active]
                            FROM dbo.TechnicalIndicator
                            WHERE InstrumentId = @InstrumentId
                              AND Active = 1;
                            ";

            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@InstrumentId", id, DbType.Int64);

                var cmd = new CommandDefinition(sql, parameters);

                return await _connection.QueryFirstOrDefaultAsync<TechnicalIndicator>(cmd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting technical indicators by Id={InstrumentId}", id);
                return null;
            }
        }

        public async Task<TechnicalIndicator?> SaveTechnicalIndicatorAsync(TechnicalIndicator technicalindic)
        {
            const string sql = @"
                            INSERT INTO [TechnicalIndicator] ([IntervalCode], [TimestampUtc], [IndicatorType], [Value1], [Value2], [Value3], [ParameterHash], [Active])
                                VALUES (@IntervalCode, @TimestampUtc, @IndicatorType, @Value1, @Value2, @Value3, @ParameterHash, 1);
                                SELECT CAST(SCOPE_IDENTITY() as
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@IntervalCode", technicalindic.IntervalCode, DbType.Int32);
                parameters.Add("@TimestampUtc", technicalindic.TimestampUtc, DbType.DateTime2);
                parameters.Add("@IndicatorType", technicalindic.IndicatorType, DbType.Int32);
                parameters.Add("@Value1", technicalindic.Value1, DbType.Decimal);
                parameters.Add("@Value2", technicalindic.Value2, DbType.Decimal);
                parameters.Add("@Value3", technicalindic.Value3, DbType.Decimal);
                parameters.Add("@ParameterHash", technicalindic.ParameterHash, DbType.String);

                var newId = await _connection.ExecuteScalarAsync<int>(sql, parameters);
                technicalindic.InstrumentId = newId;
                technicalindic.Active = true;
                return technicalindic;
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                _logger.LogWarning(sqlEx, "Duplicate active technical indicator {IntervalCode} on exchange {IndicatorType}", technicalindic.IntervalCode, technicalindic.IndicatorType);

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving technical indicator");
                return null;
            }
        }
    }
}

























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.