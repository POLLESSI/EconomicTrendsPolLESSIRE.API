using Dapper;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class UserMessageRepository : IUserMessageRepository
    {
    #nullable disable
        private readonly IDbConnection _db;
        private readonly ILogger<UserMessageRepository> _logger;

        public UserMessageRepository(IDbConnection db, ILogger<UserMessageRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<UserMessage> InsertAsync(UserMessage msg, CancellationToken ct = default)
        {
            const string sql = @"
                            INSERT INTO dbo.UserMessage (UserId, SourceType, SourceId, RelatedName, Latitude, Longitude, Tags, Content, CreatedAt, Active)
                            OUTPUT INSERTED.*
                            VALUES (@UserId, @SourceType, @SourceId, @RelatedName, @Latitude, @Longitude, @Tags, @Content, @CreatedAt, 1);
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", msg.UserId, DbType.Int64);
                parameters.Add("@SourceType", msg.SourceType, DbType.String);
                parameters.Add("@SourceId", msg.SourceId, DbType.Int32);
                parameters.Add("@RelatedName", msg.RelatedName, DbType.String);
                parameters.Add("@Latitude", msg.Latitude, DbType.Decimal);
                parameters.Add("@Longitude", msg.Longitude, DbType.Decimal);
                parameters.Add("@Tags", msg.Tags, DbType.String);
                parameters.Add("@Content", msg.Content, DbType.String);
                parameters.Add("@CreatedAt", msg.CreatedAt, DbType.DateTime2);

                return await _db.QuerySingleAsync<UserMessage>(new CommandDefinition(sql, parameters, cancellationToken: ct));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting user message");
                return null;
            }
        }

        public async Task<List<UserMessage>> GetLatestAsync(int take = 100, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP (@Take) * 
                            FROM dbo.UserMessage
                            WHERE Active = 1
                            ORDER BY CreatedAt DESC;
                            ";

            var rows = await _db.QueryAsync<UserMessage>(new CommandDefinition(sql, new { Take = take }, cancellationToken: ct));
            return rows.ToList();
        }

        public async Task<UserMessage?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            const string sql = @"SELECT * FROM dbo.UserMessage WHERE Id = @Id AND Active = 1;";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("id", id);
                return await _db.QuerySingleOrDefaultAsync<UserMessage>(new CommandDefinition(sql, parameters, cancellationToken: ct));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geting user message by Id");
                return null;
            }
            
        }

        public async Task<bool> DeleteMessageAsync(int id, CancellationToken ct = default)
        {
            const string sql = @"
                            DELETE FROM dbo.UserMessage
                            WHERE Id = @Id AND Active = 1;
                            ";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("id", id);
                // The INSTEAD OF DELETE trigger will convert this DELETE into UPDATE Active=0
                var affected = await _db.ExecuteAsync(new CommandDefinition(
                    sql,
                    parameters,
                    cancellationToken: ct));

                return affected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geting user message by Id");
                return false;
            }
            
        }
    }
}






















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.