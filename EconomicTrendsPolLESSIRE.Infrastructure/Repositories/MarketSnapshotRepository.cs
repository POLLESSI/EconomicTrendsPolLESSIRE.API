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
    public class MarketSnapshotRepository : IMarketSnapshotRepository
    {
    #nullable disable
        private readonly System.Data.IDbConnection _connection;
        private readonly ILogger<MarketSnapshotRepository> _logger;

        public MarketSnapshotRepository(IDbConnection connection, ILogger<MarketSnapshotRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<int> ArchivePastMarketSnapshotsAsync(CancellationToken ct = default)
        {
            const string sql = @"
                            UPDATE [MarketSnapshot]
                                    SET [Active] = 0
                                    WHERE [Active] = 1
                                        AND [ReceivedAtUtc] < DATEADD(DAY, -1, CAST(GETDATE() AS DATETIME2(0)));
                            ";

            try
            {
                var affectedRows = await _connection.ExecuteAsync(sql);
                _logger.LogInformation("{Count} market snapshot(s) archived.", affectedRows);
                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error archiving past market snapshots");
                return 0;
            }
        }

        public async Task<bool> DeleteMarketSnapshotAsync(long instrumentId)
        {
            const string sql = @"
                            DELETE FROM MarketSnapshot WHERE InstrumentId = @InstrumentId
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@InstrumentId", instrumentId, DbType.Int64);

                var affectedRows = await _connection.ExecuteAsync(sql, parameters);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting past market snapshots");
                return false;
            }
        }

        public Task<IEnumerable<MarketSnapshot>> GetAllMarketSnapshotAsync(int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(@Limit) [InstrumentId], [LastPrice], [BidPrice], [AskPrice], [OpenPrice], [HighPrice], [LowPrice], [PreviousClose], [Volume], [LastProviderId], [MarketTimestampUtc], [ReceivedAtUtc], [Active]
                            FROM [MarketSnapshot]
                            WHERE [Active] = 1
                            ORDER BY [ReceivedAtUtc] ASC; 
                            ";

            try
            {
                return _connection.QueryAsync<MarketSnapshot>(new CommandDefinition(sql, new { Limit = limit }, cancellationToken: ct));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error geting all market snapshots");
                return null;
            }
        }

        public async Task<MarketSnapshot?> GetMarketSnapshotByIdAsync(long instrumentId, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(1) [InstrumentId], [LastPrice], [BidPrice], [AskPrice], [OpenPrice], [HighPrice], [LowPrice], [PreviousClose], [Volume], [LastProviderId], [MarketTimestampUtc], [ReceivedAtUtc], [Active]
                            FROM dbo.MarketSnapshot
                            WHERE InstrumentId = @InstrumentId
                              AND Active = 1;
                            ";

            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@InstrumentId", instrumentId, DbType.Int64);

                var cmd = new CommandDefinition(sql, parameters);

                return await _connection.QueryFirstOrDefaultAsync<MarketSnapshot>(cmd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market snapshot by InstrumentId={InstrumentId}", instrumentId);
                return null;
            }
        }
        public async Task<MarketSnapshot> SaveMarketSnapshotAsync(MarketSnapshot marketSnpsht)
        {
            const string sql = @"
                            INSERT INTO [MarketSnapshot] ([InstrumentId], [LastPrice], [BidPrice], [AskPrice], [OpenPrice], [HighPrice], [LowPrice], [PreviousClose], [Volume], [LastProviderId], [MarketTimestampUtc], [ReceivedAtUtc], [Active])
                            VALUES (@InstrumentId, @LastPrice, @BidPrice, @AskPrice, @OpenPrice, @HighPrice, @LowPrice, @PreviousClose, @Volume, @LastProviderId, @MarketTimestampUtc, @ReceivedAtUtc, 1);
                            SELECT CAST(SCOPE_IDENTITY() as BIGINT);"";
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@InstrumentId", marketSnpsht.InstrumentId, DbType.String);
                parameters.Add("@LastPrice", marketSnpsht.LastPrice, DbType.String);
                parameters.Add("@BidPrice", marketSnpsht.BidPrice, DbType.Int32);
                parameters.Add("@AskPrice", marketSnpsht.AskPrice, DbType.String);
                parameters.Add("@OpenPrice", marketSnpsht.OpenPrice, DbType.String);
                parameters.Add("@HighPrice", marketSnpsht.HighPrice, DbType.DateTime2);
                parameters.Add("@LowPrice", marketSnpsht.LowPrice, DbType.Decimal);
                parameters.Add("@PreviousClose", marketSnpsht.PreviousClose, DbType.Decimal);
                parameters.Add("@Volume", marketSnpsht.Volume, DbType.Decimal);
                parameters.Add("@LastProviderId", marketSnpsht.LastProviderId, DbType.Int32);
                parameters.Add("@MarketTimestampUtc", marketSnpsht.MarketTimestampUtc, DbType.DateTime2);
                parameters.Add("@ReceivedAtUtc", marketSnpsht.ReceivedAtUtc, DbType.DateTime2);

                var newId = await _connection.ExecuteScalarAsync<int>(sql, parameters);
                marketSnpsht.InstrumentId = newId;
                marketSnpsht.Active = true;
                return marketSnpsht;
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                _logger.LogWarning(sqlEx, "Duplicate active market snapshot {InstrumentId} on exchange {LastPrice}", marketSnpsht.InstrumentId, marketSnpsht.LastPrice);

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Market Snapshot");
                return null;
            }
        }
    }
}

















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.