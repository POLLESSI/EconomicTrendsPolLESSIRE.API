namespace EconomicTrendsPolLESSIRE.Application.Interfaces.ExternalProviders
{
    public interface IMongoConnectionStringProvider
    {
        Task<string> GetConnectionStringAsync(CancellationToken ct = default);
    }
}
