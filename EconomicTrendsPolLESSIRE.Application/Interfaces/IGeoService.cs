namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface IGeoService
    {
        Task<(double Latitude, double Longitude)?> GetCoordinatesAsync(string location);
    }
}
