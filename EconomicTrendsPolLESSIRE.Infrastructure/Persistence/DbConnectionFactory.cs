using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Persistence
{
    public sealed class DbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection CreateConnection()
        {
            var cs =
                _configuration.GetConnectionString("default")
                ?? _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(cs))
            {
                throw new InvalidOperationException(
                    "SQL ConnectionString missing. Expected ConnectionStrings:default or ConnectionStrings:DefaultConnection.");
            }

            Console.WriteLine($"[DB-FACTORY] SQL connection created. Length={cs.Length}");

            return new SqlConnection(cs);
        }
    }
}