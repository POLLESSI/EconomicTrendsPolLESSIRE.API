using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Abstractions;
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Collections;
using MongoDB.Driver;

namespace EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Repositories
{
    public sealed class MistralInteractionNoSqlRepository
    {
        private readonly IMongoCollection<MistralInteractionDocument> _collection;

        public MistralInteractionNoSqlRepository(IMongoDbContext context)
        {
            _collection = context.Collection<MistralInteractionDocument>("mistral_interactions");
        }

        public Task InsertAsync(MistralInteractionDocument document, CancellationToken ct = default)
            => _collection.InsertOneAsync(document, cancellationToken: ct);

        public async Task<IReadOnlyList<MistralInteractionDocument>> GetLatestAsync(int limit, CancellationToken ct = default)
        {
            return await _collection
                .Find(_ => true)
                .SortByDescending(x => x.CreatedAtUtc)
                .Limit(limit)
                .ToListAsync(ct);
        }
    }
}







































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.