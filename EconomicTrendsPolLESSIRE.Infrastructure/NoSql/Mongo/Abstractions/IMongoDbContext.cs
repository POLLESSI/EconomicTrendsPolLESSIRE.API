using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Abstractions
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }
        IMongoCollection<T> Collection<T>(string name);
    }
}





































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.