using EconomicTrendsPolLESSIRE.Shared.Options;

namespace EconomicTrendsPolLESSIRE.Shared.Time
{
    public interface IDelayHelper
    {
        static abstract TimeSpan GetDelayUntilNextRun(DailyArchiverOptions o);
    }
}
