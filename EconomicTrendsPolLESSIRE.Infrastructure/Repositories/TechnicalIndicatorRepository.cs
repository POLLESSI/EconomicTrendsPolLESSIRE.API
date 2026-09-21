using Dapper;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class TechnicalIndicatorRepository : ITechnicalIndicatorRepository
    {
#nullable disable

        private readonly IDbConnection _connection;
        private readonly ILogger<TechnicalIndicatorRepository> _logger;

        public TechnicalIndicatorRepository(
            IDbConnection connection,
            ILogger<TechnicalIndicatorRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<int> ArchivePastTechnicalIndicatorsAsync(CancellationToken ct = default)
        {
            const string sql = @"
                             UPDATE dbo.[TechnicalIndicator]
                             SET [Active] = 0
                             WHERE [Active] = 1
                             AND [TimestampUtc] <DATEADD(DAY, -1, CAST(SYSUTCDATETIME() AS DATETIME2(0)));
                            ";

            try
            {
                var cmd = new CommandDefinition(sql, cancellationToken: ct);
                var affectedRows = await _connection.ExecuteAsync(cmd);

                _logger.LogInformation("{Count} technical indicator(s) archived.", affectedRows);

                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error archiving past technical indicators");

                return 0;
            }
        }

        public async Task<bool> DeleteTechnicalIndicatorAsync(long instrumentId, int intervalCode, int indicatorType, DateTime timestampUtc, CancellationToken ct = default)
        {
            const string sql = @"
                            DELETE FROM dbo.[TechnicalIndicator]
                            WHERE [InstrumentId] = @InstrumentId
                              AND [IntervalCode] = @IntervalCode
                              AND [IndicatorType] = @IndicatorType
                              AND [TimestampUtc] = @TimestampUtc;
                            ";

            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@InstrumentId", instrumentId, DbType.Int64);
                parameters.Add("@IntervalCode", intervalCode, DbType.Int32);
                parameters.Add("@IndicatorType", indicatorType, DbType.Int32);
                parameters.Add("@TimestampUtc", timestampUtc, DbType.DateTime2);

                var cmd = new CommandDefinition(sql, parameters, cancellationToken: ct);
                var affectedRows = await _connection.ExecuteAsync(cmd);

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting TechnicalIndicator {InstrumentId}/{IntervalCode}/{IndicatorType}/{TimestampUtc}", instrumentId, intervalCode, indicatorType, timestampUtc);

                return false;
            }
        }

        public Task<bool> DeleteTechnicalIndicatorAsync(long instrumentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TechnicalIndicator>> GetAllTechnicalIndicatorAsync(int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(@Limit) [InstrumentId], [IntervalCode], [TimestampUtc], [IndicatorType], [Value1], [Value2], [Value3], [ParameterHash], [Active]
                            FROM dbo.[TechnicalIndicator]
                            WHERE [Active] = 1
                            ORDER BY [TimestampUtc] ASC;
                            ";

            var cmd = new CommandDefinition(sql, new { Limit = limit }, cancellationToken: ct);

            return _connection.QueryAsync<TechnicalIndicator>(cmd);
        }

        public async Task<TechnicalIndicator?> GetTechnicalIndicatorByIdAsync(long instrumentId, int intervalCode, int indicatorType, DateTime timestampUtc, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(1) [InstrumentId], [IntervalCode], [TimestampUtc], [IndicatorType], [Value1], [Value2],  [Value3], [ParameterHash], [Active]
                            FROM dbo.[TechnicalIndicator]
                            WHERE [InstrumentId] = @InstrumentId
                              AND [IntervalCode] = @IntervalCode
                              AND [IndicatorType] = @IndicatorType
                              AND [TimestampUtc] = @TimestampUtc
                              AND [Active] = 1;
                            ";

            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@InstrumentId", instrumentId, DbType.Int64);
                parameters.Add("@IntervalCode", intervalCode, DbType.Int32);
                parameters.Add("@IndicatorType", indicatorType, DbType.Int32);
                parameters.Add("@TimestampUtc", timestampUtc, DbType.DateTime2);

                var cmd = new CommandDefinition(sql, parameters, cancellationToken: ct);

                return await _connection.QueryFirstOrDefaultAsync<TechnicalIndicator>(cmd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting TechnicalIndicator {InstrumentId}/{IntervalCode}/{IndicatorType}/{TimestampUtc}", instrumentId, intervalCode, indicatorType, timestampUtc);

                return null;
            }
        }

        public async Task<TechnicalIndicator?> SaveTechnicalIndicatorAsync(TechnicalIndicator technicalindic)
        {
            const string sql = @"
                            INSERT INTO dbo.TechnicalIndicator([InstrumentId], [IntervalCode], [TimestampUtc], [IndicatorType], [Value1], [Value2], [Value3], [ParameterHash], [Active])
                            VALUES(@InstrumentId, @IntervalCode, @TimestampUtc, @IndicatorType, @Value1, @Value2, @Value3, @ParameterHash, 1);
                            ";

            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@InstrumentId", technicalindic.InstrumentId, DbType.Int64);
                parameters.Add("@IntervalCode", technicalindic.IntervalCode, DbType.Int32);
                parameters.Add("@TimestampUtc", technicalindic.TimestampUtc, DbType.DateTime2);
                parameters.Add("@IndicatorType", technicalindic.IndicatorType, DbType.Int32);
                parameters.Add("@Value1", technicalindic.Value1, DbType.Decimal);
                parameters.Add("@Value2", technicalindic.Value2, DbType.Decimal);
                parameters.Add("@Value3", technicalindic.Value3, DbType.Decimal);
                parameters.Add("@ParameterHash", technicalindic.ParameterHash, DbType.Byte);

                var affectedRows = await _connection.ExecuteAsync(sql, parameters);

                if (affectedRows != 1)
                {
                    return null;
                }

                technicalindic.Active = true;

                return technicalindic;
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                _logger.LogWarning(sqlEx, "Duplicate TechnicalIndicator {InstrumentId}/{IntervalCode}/{IndicatorType}/{TimestampUtc}", technicalindic.InstrumentId, technicalindic.IntervalCode, technicalindic.IndicatorType, technicalindic.TimestampUtc);

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving TechnicalIndicator");

                return null;
            }
        }
    }
}

























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.