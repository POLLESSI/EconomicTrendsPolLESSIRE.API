namespace EconomicTrendsPolLESSIRE.Shared.Interfaces
{
    public interface IDailyArchiverOptions
    {
        string TimeZone { get; }
        int Hour { get; }
        int Minute { get; }
    }
}
