using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Metrics;
using Microsoft.Data.SqlClient;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class MarketCandleRepository : IMarketCandleRepository
    {
    #nullable disable
        private readonly System.Data.IDbConnection _connection;
        private readonly ILogger<MarketCandleRepository> _logger;

        public MarketCandleRepository(IDbConnection connection, ILogger<MarketCandleRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<int> ArchivePastMarketCandlesAsync(CancellationToken ct = default)
        {
            const string sql = @"
                                UPDATE [MarketCandle]
                                SET [Active] = 0
                                WHERE [Active] = 1
                                    AND [OpenTimeUtc] < DATEADD(DAY, -1, CAST(GETDATE() AS DATETIME2(0)));";
            try
            {
                var cmd = new CommandDefinition(sql, cancellationToken: ct);

                var affectedRows = await _connection.ExecuteAsync(cmd);
                _logger.LogInformation("{Count} market candle(s) archived.", affectedRows);
                return affectedRows;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error archiving past market candles");
                return 0;
            }
        }

        public async Task<bool> DeleteMarketCandleAsync(long instrumentId)
        {
            const string sql = @"DELETE FROM MarketCandle WHERE InstrumentId = @InstrumentId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("InstrumentId", instrumentId, DbType.Int32);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public Task<IEnumerable<MarketCandle>> GetAllMarketCandleAsync(int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(@Limit) [InstrumentId], [IntervalCode], [OpenTimeUtc], [OpenPrice], [HighPrice], [LowPrice], [ClosePrice], [Volume], [VWAP], [TradeCount], [IsFinal], [Active]
                            FROM [MarketCandle]
                            WHERE [Active] = 1
                            ORDER BY [OpenTimeUtc] ASC;
                            ";

            return _connection.QueryAsync<MarketCandle>(new CommandDefinition(sql, new { Limit = limit }, cancellationToken: ct));
        }

        public async Task<MarketCandle?> GetMarketCandleByIdAsync(long instrumentId, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(1) [InstrumentId], [IntervalCode], [OpenTimeUtc], [OpenPrice], [HighPrice], [LowPrice], [ClosePrice], [Volume], [VWAP], [TradeCount], [IsFinal], [Active]
                            FROM dbo.MarketCandle
                            WHERE [InstrumentId] = @InstrumentId
                                AND Active = 1;
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@InstrumentId", instrumentId, DbType.Int64);

                var cmd = new CommandDefinition(sql, parameters, cancellationToken: ct);
                return await _connection.QueryFirstOrDefaultAsync<MarketCandle>(cmd);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error getting Market Candels by Id={Id}", instrumentId);
                return null;
            }
        }

        public async Task<MarketCandle?> SaveMarketCandleAsync(MarketCandle marketCdl)
        {
            const string sql = @"
                            INSERT INTO [MarketCandle] ([InstrumentId], [IntervalCode], [OpenTimeUtc], [OpenPrice], [HighPrice], [LowPrice], [ClosePrice], [Volume], [VWAP], [TradeCount], [IsFinal], [Active])
                            VALUES (@InstrumentId, @IntervalCode, @OpenTimeUtc, @OpenPrice, @HighPrice, @LowPrice, @ClosePrice, @Volume, @VWAP, @TradeCount, @IsFinal, 1);
                            SELECT CAST(SCOPE_IDENTITY() as BIGINT);
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@InstrumentId", marketCdl.InstrumentId, DbType.Int64);
                parameters.Add("@IntervalCode", marketCdl.IntervalCode, DbType.Int64);
                parameters.Add("@OpenTimeUtc", marketCdl.OpenTimeUtc, DbType.DateTime2);
                parameters.Add("@OpenPrice", marketCdl.OpenPrice, DbType.Decimal);
                parameters.Add("@HighPrice", marketCdl.HighPrice, DbType.Decimal);
                parameters.Add("@LowPrice", marketCdl.LowPrice, DbType.Decimal);
                parameters.Add("@ClosePrice", marketCdl.ClosePrice, DbType.Decimal);
                parameters.Add("@Volume", marketCdl.Volume, DbType.Decimal);
                parameters.Add("@VWAP", marketCdl.VWAP, DbType.Decimal);
                parameters.Add("@TradeCount", marketCdl.TradeCount, DbType.Int64);
                parameters.Add("@IsFinal", marketCdl.IsFinal, DbType.Boolean);

                var newId = await _connection.ExecuteScalarAsync<int>(sql, parameters);
                marketCdl.InstrumentId = newId;
                marketCdl.Active = true;
                return marketCdl;
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                _logger.LogWarning(sqlEx, "Duplicaye active market candle {InstrumentId} on exchange {IntervalCode}", marketCdl.InstrumentId, marketCdl.IntervalCode);
                return null;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error saving Market Candles");
                return null;
            }
            
        }
    }
}












































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.