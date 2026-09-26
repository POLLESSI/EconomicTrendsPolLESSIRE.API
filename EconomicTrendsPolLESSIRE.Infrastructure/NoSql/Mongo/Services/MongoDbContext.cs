using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Abstractions;
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Services
{
    public sealed class MongoDbContext : IMongoDbContext
    {
        public IMongoDatabase Database { get; }

        public MongoDbContext(IOptions<MongoOptions> options)
        {
            var opt = options.Value;

            if (string.IsNullOrWhiteSpace(opt.ConnectionString))
                throw new InvalidOperationException("MongoDb:ConnectionString is missing.");

            if (string.IsNullOrWhiteSpace(opt.DatabaseName))
                throw new InvalidOperationException("MongoDb:DatabaseName is missing.");

            var client = new MongoClient(opt.ConnectionString);
            Database = client.GetDatabase(opt.DatabaseName);
        }

        public IMongoCollection<T> Collection<T>(string name)
            => Database.GetCollection<T>(name);
    }
}















































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.