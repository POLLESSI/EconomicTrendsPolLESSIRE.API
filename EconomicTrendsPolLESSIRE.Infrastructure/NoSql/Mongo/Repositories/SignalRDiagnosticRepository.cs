using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Abstractions;
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Collections;
using MongoDB.Driver;

namespace EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Repositories
{
    public sealed class SignalRDiagnosticRepository : ISignalRDiagnosticRepository
    {
        private readonly IMongoCollection<SignalRDiagnosticDocument> _collection;

        public SignalRDiagnosticRepository(IMongoDbContext context)
        {
            _collection = context.Collection<SignalRDiagnosticDocument>("signalr_diagnostics");
        }

        public Task InsertAsync(
            SignalRDiagnosticDocument document,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(document);

            return _collection.InsertOneAsync(document, cancellationToken: ct);
        }
    }
}




















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.