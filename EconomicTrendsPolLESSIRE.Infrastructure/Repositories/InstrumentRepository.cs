using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using Instrument = EconomicTrendsPolLESSIRE.Domain.Entities.Instrument;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Repositories
{
    public class InstrumentRepository : IInstrumentRepository
    {
    #nullable disable
        private readonly IDbConnection _connection;
        private readonly ILogger<InstrumentRepository> _logger;

        public InstrumentRepository(IDbConnection connection, ILogger<InstrumentRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<int> ArchivePastInstrumentsAsync(CancellationToken ct = default)
        {
            const string sql = @"
                                    UPDATE [Instrument]
                                    SET [Active] = 0
                                    WHERE [Active] = 1
                                        AND [CreatedAtUtc] < DATEADD(DAY, -1, CAST(GETDATE() AS DATETIME2(0)));
                            ";
            try
            {
                var affectedRows = await _connection.ExecuteAsync(sql);
                _logger.LogInformation("{Count} instrument(s) archived.", affectedRows);
                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error archiving past instruments");
                return 0;
            }
        }

        public async Task<bool> DeleteInstrumentAsync(long id)
        {
            var sql = "DELETE FROM Instrument WHERE Id = @Id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int64);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public Task<IEnumerable<Instrument>> GetAllInstrumentAsync(int limit = 200, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT TOP(@Limit) [Id], [Symbol], [Name], [AssetClass], [ExchangeCode], [CurrencyCode], [CreatedAtUtc], [Active]
                            FROM [Instrument]
                            WHERE [Active] = 1
                            ORDER BY [CreatedAtUtc] ASC; 
                            ";

            return _connection.QueryAsync<Instrument>(new CommandDefinition(sql, new { Limit = limit}, cancellationToken: ct));
        }

        public async Task<Instrument?> GetInstrumentByIdAsync(long id, CancellationToken ct = default)
        {
            const string sql = """
                            SELECT TOP(1) [Id], [Symbol], [Name], [AssetClass], [ExchangeCode], [CurrencyCode], [CreatedAtUtc], [Active]
                            FROM dbo.Instrument
                            WHERE Id = @Id
                              AND Active = 1;
                            """;

            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", id, DbType.Int64);

                var cmd = new CommandDefinition(sql, parameters, cancellationToken: ct);

                return await _connection.QueryFirstOrDefaultAsync<Instrument>(cmd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Instrument by Id={Id}", id);
                return null;
            }
        }

        public async Task<Instrument?> SaveInstrumentAsync(Instrument instrument)
        {
            const string sql = @"
                                INSERT INTO [Instrument] ([Symbol], [Name], [AssetClass], [ExchangeCode], [CurrencyCode], [CreatedAtUtc], [Active])
                                VALUES (@Symbol, @Name, @AssetClass, @ExchangeCode, @CurrencyCode, @CreatedAtUtc, 1);
                                SELECT CAST(SCOPE_IDENTITY() as BIGINT);";

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Symbol", instrument.Symbol, DbType.String);
                parameters.Add("@Name", instrument.Name, DbType.String);
                parameters.Add("@AssetClass", instrument.AssetClass, DbType.Int32);
                parameters.Add("@ExchangeCode", instrument.ExchangeCode, DbType.String);
                parameters.Add("@CurrencyCode", instrument.CurrencyCode, DbType.String);
                parameters.Add("@CreatedAtUtc", instrument.CreatedAtUtc, DbType.DateTime2);

                var newId = await _connection.ExecuteScalarAsync<int>(sql, parameters);
                instrument.Id = newId;
                instrument.Active = true;
                return instrument;
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                _logger.LogWarning(sqlEx, "Duplicate active instrument {Symbol} on exchange {ExchangeCode}", instrument.Symbol, instrument.ExchangeCode);

                return null;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error saving Instrument");
                return null;
            }
        }

        public async Task<Instrument> UpsertByNaturalKeyAsync(string symbol, string name, int assetClass, string? exchangeCode, string? currencyCode, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(symbol);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            symbol = symbol.Trim().ToUpperInvariant();

            const string sql = @"
                            UPDATE dbo.Instrument
                            SET
                                Name = @Name,
                                AssetClass = @AssetClass,
                                CurrencyCode = @CurrencyCode,
                                Active = 1
                            WHERE Symbol = @Symbol
                              AND
                              (
                                  ExchangeCode = @ExchangeCode
                                  OR
                                  (ExchangeCode IS NULL AND @ExchangeCode IS NULL)
                              );

                            IF @@ROWCOUNT = 0
                            BEGIN
                                INSERT INTO dbo.Instrument
                                (
                                    Symbol,
                                    Name,
                                    AssetClass,
                                    ExchangeCode,
                                    CurrencyCode,
                                    Active
                                )
                                VALUES
                                (
                                    @Symbol,
                                    @Name,
                                    @AssetClass,
                                    @ExchangeCode,
                                    @CurrencyCode,
                                    1
                                );
                            END;

                            SELECT TOP (1)
                                Id,
                                Symbol,
                                Name,
                                AssetClass,
                                ExchangeCode,
                                CurrencyCode,
                                CreatedAtUtc,
                                Active
                            FROM dbo.Instrument
                            WHERE Symbol = @Symbol
                              AND
                              (
                                  ExchangeCode = @ExchangeCode
                                  OR
                                  (ExchangeCode IS NULL AND @ExchangeCode IS NULL)
                              )
                            ORDER BY Id DESC;
                            ";

            return await _connection.QuerySingleAsync<Instrument>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        Symbol = symbol,
                        Name = name.Trim(),
                        AssetClass = assetClass,
                        ExchangeCode = exchangeCode,
                        CurrencyCode = currencyCode
                    },
                    cancellationToken: ct));
        }
    }
}
















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.