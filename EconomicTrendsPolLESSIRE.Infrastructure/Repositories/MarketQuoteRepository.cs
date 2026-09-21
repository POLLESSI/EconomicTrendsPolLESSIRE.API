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
    public class MarketQuoteRepository : IMarketQuoteRepository
    {
    #nullable disable
        private readonly System.Data.IDbConnection _connection;
        private readonly ILogger<MarketQuoteRepository> _logger;

        public MarketQuoteRepository(IDbConnection connection, ILogger<MarketQuoteRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<int> ArchivePastMarketQuotesAsync(CancellationToken ct = default)
        {
            const string sql = @"UPDATE [Instrument]
                                    SET [Active] = 0
                                    WHERE [Active] = 1
                                        AND [ReceivedAtUtc] < DATEADD(DAY, -1, CAST(GETDATE() AS DATETIME2(0)));"";";
            try
            {
                var affectedRows = await _connection.ExecuteAsync(sql);
                _logger.LogInformation("{Count} market quote(s) archived.", affectedRows);
                return affectedRows;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error archiving past market quotes");
                return 0;
            }
        }

        public async Task<bool> DeleteMarketQuoteAsync(long id)
        {
            const string sql = @"DELETE FROM MarketQuote WHERE Id = @Id";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int64);

                var affectedRows = await _connection.ExecuteAsync(sql, parameters);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error deleting past market quote");
                return false;
            }
        }

        public Task<IEnumerable<MarketQuote>> GetAllMarketQuoteAsync(int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(@Limit) [Id], [InstrumentId], [ProviderId], [TimestampUtc], [ReceivedAtUtc], [BidPrice], [BidSize], [AskPrice], [AskSize], [Active]
                            FROM [MarketQuote]
                            WHERE [Active] = 1
                            ORDER BY [ReceivedAtUtc] ASC; "";
                            ";

            return _connection.QueryAsync<MarketQuote>(new CommandDefinition(sql, new { Limit = limit }, cancellationToken: ct));
        }

        public async Task<MarketQuote?> GetMarketQuoteByIdAsync(long id, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(1) [Id], [InstrumentId], [ProviderId], [TimestampUtc], [ReceivedAtUtc], [BidPrice], [BidSize], [AskPrice], [AskSize], [Active]
                            FROM dbo.MarketQuote
                            WHERE Id = @Id
                              AND Active = 1;
                            ";

            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", id, DbType.Int64);

                var cmd = new CommandDefinition(sql, parameters);

                return await _connection.QueryFirstOrDefaultAsync<MarketQuote>(cmd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market quote by Id={Id} async", id);
                return null;
            }
        }

        public async Task<MarketQuote?> SaveMarketQuoteAsync(MarketQuote marketQt)
        {
            const string sql = @"
                            INSERT INTO [MarketQuote] ([InstrumentId], [ProviderId], [TimestampUtc], [ReceivedAtUtc], [BidPrice], [BidSize], [AskPrice], [AskSize], [Active])
                            VALUES (@InstrumentId, @ProviderId, @TimestampUtc, @ReceivedAtUtc, @BidPrice, @BidSize, @AskPrice, @AskSize, 1);
                            SELECT CAST(SCOPE_IDENTITY() as BIGINT);"";
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@InstrumentId", marketQt.InstrumentId, DbType.Int64);
                parameters.Add("@ProviderId", marketQt.ProviderId, DbType.Int32);
                parameters.Add("@TimestampUtc", marketQt.TimestampUtc, DbType.DateTime2);
                parameters.Add("@ReceivedAtUtc", marketQt.ReceivedAtUtc, DbType.DateTime2);
                parameters.Add("@BidPrice", marketQt.BidPrice, DbType.Decimal);
                parameters.Add("@BidSize", marketQt.BidSize, DbType.Decimal);
                parameters.Add("@AskPrice", marketQt.AskPrice, DbType.Decimal);
                parameters.Add("@AskSize", marketQt.AskSize, DbType.Decimal);

                var newId = await _connection.ExecuteScalarAsync<int>(sql, parameters);
                marketQt.Id = newId;
                marketQt.Active = true;
                return marketQt;
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                //_logger.LogWarning(sqlEx, "Duplicate active market quote {Symbol} on exchange {ExchangeCode}", MarketQuote.InstrumentId, MarketQuote.BidPrice);

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving market quote");
                return null;
            }
        }
    }
}
















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.