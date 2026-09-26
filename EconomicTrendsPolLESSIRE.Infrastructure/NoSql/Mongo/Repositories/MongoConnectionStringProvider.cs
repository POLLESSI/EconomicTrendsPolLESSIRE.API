using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Abstractions;
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Options;
using Microsoft.Extensions.Options;

namespace EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Repositories
{
    public sealed class MongoConnectionStringProvider
    {
        private readonly MongoOptions _options;

        public MongoConnectionStringProvider(IOptions<MongoOptions> options)
        {
            _options = options.Value;
        }

        public string GetConnectionString()
        {
            if (!string.IsNullOrWhiteSpace(_options.ConnectionString))
                return _options.ConnectionString;

            throw new InvalidOperationException(
                "MongoDb:ConnectionString is missing. In production, inject it via Key Vault reference or environment variable MongoDb__ConnectionString.");
        }
    }
}














































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.