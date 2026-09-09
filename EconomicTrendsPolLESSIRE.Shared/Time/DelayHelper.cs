using EconomicTrendsPolLESSIRE.Shared.Interfaces;
using EconomicTrendsPolLESSIRE.Shared.Options;
using System;

namespace EconomicTrendsPolLESSIRE.Shared.Time
{
    public static class DelayHelper
    {
        public static TimeSpan GetDelayUntilNextRun(IDailyArchiverOptions o)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(o.TimeZone);
            var nowLocal = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz);
            var nextLocal = new DateTimeOffset(nowLocal.Year, nowLocal.Month, nowLocal.Day, o.Hour, o.Minute, 0, nowLocal.Offset);
            if (nowLocal >= nextLocal) nextLocal = nextLocal.AddDays(1);
            var nextUtc = nextLocal.ToUniversalTime();
            return nextUtc - DateTimeOffset.UtcNow;
        }
    }
}
