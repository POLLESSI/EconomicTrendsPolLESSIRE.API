using EconomicTrendsPolLESSIRE.Application.Extensions;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Enums;
using EconomicTrendsPolLESSIRE.Domain.Entities;
//using EconomicTrendsPolLESSIRE.Domain.ValueObjects;
//using EconomicTrendsPolLESSIRE.DTOs.DTOs;
using Volo.Abp.Domain.Entities;

namespace EconomicTrendsPolLESSIRE.Application.Extensions
{
    public class MapperExtensions
    {
#nullable disable
        public static decimal RoundLat(double lat) => Math.Round((decimal)lat, 6);
        public static decimal RoundLon(double lon) => Math.Round((decimal)lon, 6);
        public static DateTime TruncateToSecond(DateTime dt)
        {
            var ticks = dt.Ticks - (dt.Ticks % TimeSpan.TicksPerSecond);
            return new DateTime(ticks, dt.Kind);
        }
        private static DateTimeOffset TruncateToSecond(DateTimeOffset dto)
        {
            var ticks = dto.UtcTicks - (dto.UtcTicks % TimeSpan.TicksPerSecond);
            return new DateTimeOffset(ticks, TimeSpan.Zero); // standardized UTC
        }
    }
}
