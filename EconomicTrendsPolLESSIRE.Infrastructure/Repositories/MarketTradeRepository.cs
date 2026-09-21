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
    public class MarketTradeRepository : IMarketTradeRepository
    {
        #nullable disable
        private readonly System.Data.IDbConnection _connection;
        private readonly ILogger<MarketTradeRepository> _logger;

        public MarketTradeRepository(IDbConnection connection, ILogger<MarketTradeRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<int> ArchivePastMarketTradesAsync(CancellationToken ct = default)
        {
            const string sql = @"
                            UPDATE [MarketTrade]
                                    SET [Active] = 0
                                    WHERE [Active] = 1
                                        AND [ReceivedAtUtc] < DATEADD(DAY, -1, CAST(GETDATE() AS DATETIME2(0)));
                            ";

            try
            {
                var affectedRows = await _connection.ExecuteAsync(sql);
                _logger.LogInformation("{Count} market trade(s) archived.", affectedRows);
                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error archiving past market trades");
                return 0;
            }
        }

        public async Task<bool> DeleteMarketTradeAsync(long id)
        {
            const string sql = @"
                            DELETE FROM MarketTrade WHERE Id = @Id
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
                _logger.LogError(ex, "Error deleting past market trades");
                return false;
            }
        }

        public Task<IEnumerable<MarketTrade>> GetAllMarketTradeAsync(int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(@Limit) [Id], [InstrumentId], [ProviderId], [TimestampUtc], [ReceivedAtUtc], [Price], [Quantity], [SequenceNumber], [Active]
                            FROM [MarketTrade]
                            WHERE [Active] = 1
                            ORDER BY [ReceivedAtUtc] ASC;
                            ";

            try
            {
                return _connection.QueryAsync<MarketTrade>(new CommandDefinition(sql, new { Limit = limit }, cancellationToken: ct));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error get all market trades");
                return null;
            }
        }

        public async Task<MarketTrade?> GetMarketTradeByIdAsync(long id, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(1) [Id], [InstrumentId], [ProviderId], [TimestampUtc], [ReceivedAtUtc], [Price], [Quantity], [SequenceNumber], [Active]
                            FROM dbo.MarketTrade
                            WHERE Id = @Id
                              AND Active = 1;
                            ";

            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", id, DbType.Int64);

                var cmd = new CommandDefinition(sql, parameters);

                return await _connection.QueryFirstOrDefaultAsync<MarketTrade>(cmd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market trade by Id={Id}", id);
                return null;
            }
        }

        public async Task<MarketTrade?> SaveMarketTradeAsync(MarketTrade markettrd)
        {
            const string sql = @"
                            INSERT INTO [MarketTrade] ([InstrumentId], [ProviderId], [TimestampUtc], [ReceivedAtUtc], [Price], [Quantity], [SequenceNumber], [Active])
                            VALUES (@InstrumentId, @ProviderId, @TimestampUtc, @ReceivedAtUtc, @Price, @Quantity, @SequenceNumber, 1);
                            SELECT CAST(SCOPE_IDENTITY() as BIGINT);
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@InstrumentId", markettrd.InstrumentId, DbType.Int64);
                parameters.Add("@ProviderId", markettrd.ProviderId, DbType.Int32);
                parameters.Add("@TimestampUtc", markettrd.TimestampUtc, DbType.DateTime2);
                parameters.Add("@ReceivedAtUtc", markettrd.ReceivedAtUtc, DbType.DateTime2);
                parameters.Add("@Price", markettrd.Price, DbType.Decimal);
                parameters.Add("@Quantity", markettrd.Quantity, DbType.Decimal);
                parameters.Add("@SequenceNumber", markettrd.SequenceNumber, DbType.Int32);

                var newId = await _connection.ExecuteScalarAsync<int>(sql, parameters);
                markettrd.Id = newId;
                markettrd.Active = true;
                return markettrd;
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                _logger.LogWarning(sqlEx, "Duplicate active market trade {InstrumentId} on exchange {Price}", markettrd.InstrumentId, markettrd.Price);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Market Trade");
                return null;
            }
        }
    }
}
















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.