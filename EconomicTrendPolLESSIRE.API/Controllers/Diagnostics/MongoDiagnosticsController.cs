#if DEBUG
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Abstractions;
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EconomicTrendsPolLESSIRE.API.Controllers.Diagnostics
{
    [Route("api/diagnostics/mongo")]
    [ApiController]
    public sealed class MongoDiagnosticsController : ControllerBase
    {
        //private readonly IWeatherSnapshotRepository _weatherRepository;
        //private readonly ICrowdSnapshotRepository _crowdRepository;

        //public MongoDiagnosticsController(
        //    IWeatherSnapshotRepository weatherRepository,
        //    ICrowdSnapshotRepository crowdRepository)
        //{
        //    _weatherRepository = weatherRepository;
        //    _crowdRepository = crowdRepository;
        //}

#if DEBUG
//        [AllowAnonymous]
//#else
//        [Authorize(Roles = "Admin")]
//#endif
//        [HttpPost("weather-test")]
//        public async Task<IActionResult> WriteWeatherTest(CancellationToken ct)
//        {
//            var snapshot = new WeatherSnapshotDocument
//            {
//                WeatherForecastId = 999,
//                PlaceId = 1,
//                PlaceName = "Mongo Swagger Test",
//                Latitude = 50.467388,
//                Longitude = 4.871985,
//                TemperatureC = 18.5,
//                WeatherType = "Test",
//                Severity = "Info",
//                Provider = "Swagger",
//                Summary = "MongoDB diagnostic test",
//                Description = "Inserted from Swagger diagnostic endpoint.",
//                ForecastAtUtc = DateTime.UtcNow,
//                CreatedAtUtc = DateTime.UtcNow
//            };

//            await _weatherRepository.InsertAsync(snapshot, ct);

//            return Ok(new
//            {
//                Ok = true,
//                Collection = "weather_snapshots",
//                InsertedId = snapshot.Id.ToString()
//            });
//        }

//#if DEBUG
//        [AllowAnonymous]
//#else
//        [Authorize(Roles = "Admin")]
//#endif
//        [HttpPost("crowd-test")]
//        public async Task<IActionResult> WriteCrowdTest(CancellationToken ct)
//        {
//            var snapshot = new CrowdSnapshotDocument
//            {
//                CrowdInfoId = 999,
//                PlaceId = 1,
//                PlaceName = "Mongo Swagger Test",
//                Latitude = 50.467388,
//                Longitude = 4.871985,
//                CurrentCount = 42,
//                Capacity = 100,
//                DensityRatio = 0.42,
//                CrowdLevel = "Moderate",
//                Source = "Swagger",
//                Message = "MongoDB crowd diagnostic test",
//                SnapshotAtUtc = DateTime.UtcNow,
//                CreatedAtUtc = DateTime.UtcNow
//            };

//            await _crowdRepository.InsertAsync(snapshot, ct);

//            return Ok(new
//            {
//                Ok = true,
//                Collection = "crowd_snapshots",
//                InsertedId = snapshot.Id.ToString()
//            });
//        }
//#if DEBUG
//        [HttpPost("traffic-test")]
//        public async Task<IActionResult> WriteTrafficTest(
//            [FromServices] ITrafficSnapshotRepository repository,
//            CancellationToken ct)
//        {
//            var document = new TrafficSnapshotDocument
//            {
//                TrafficConditionId = null,
//                Source = "Swagger",
//                RoadName = "N90",
//                Severity = "2",
//                Status = "Moderate",
//                Description = "Diagnostic traffic snapshot",
//                Latitude = 50.467388,
//                Longitude = 4.871985,
//                ObservedAtUtc = DateTime.UtcNow,
//                CreatedAtUtc = DateTime.UtcNow
//            };

//            await repository.InsertAsync(document, ct);

//            return Ok(new
//            {
//                Ok = true,
//                Collection = "traffic_snapshots",
//                InsertedId = document.Id.ToString()
//            });
//        }
#endif
    }
}
#endif