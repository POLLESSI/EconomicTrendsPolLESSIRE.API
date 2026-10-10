using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Abstractions;
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Collections;
using MongoDB.Driver;

namespace EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Repositories
{
    public sealed class MistralInteractionNoSqlRepository : IMistralInteractionNoSqlRepository
    {
        private readonly IMongoCollection<MistralInteractionDocument> _collection;

        public MistralInteractionNoSqlRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<MistralInteractionDocument>("MistralInteractions");
        }

        public async Task InsertAsync(
            MistralInteractionDocument document,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(document);

            await _collection.InsertOneAsync(
                document,
                cancellationToken: ct);
        }

        public async Task<IReadOnlyList<MistralInteractionDocument>>
            GetLatestAsync(
                int limit,
                CancellationToken ct = default)
        {
            limit = Math.Clamp(limit, 1, 500);

            var documents =
                await _collection
                    .Find(
                        Builders<MistralInteractionDocument>
                            .Filter
                            .Empty)
                    .Sort(
                        Builders<MistralInteractionDocument>
                            .Sort
                            .Descending("_id"))
                    .Limit(limit)
                    .ToListAsync(ct);

            return documents;
        }
    }
}







































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.