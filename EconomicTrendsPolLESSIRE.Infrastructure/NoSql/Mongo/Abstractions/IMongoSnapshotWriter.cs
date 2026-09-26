using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Collections;

namespace EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Abstractions
{
    public interface IMongoSnapshotWriter
    {
        Task WriteMistralInteractionAsync(MistralInteractionDocument document, CancellationToken ct = default);
        //Task WriteWeatherSnapshotAsync(WeatherSnapshotDocument snapshot, CancellationToken ct = default);
        //Task WriteTrafficSnapshotAsync(TrafficSnapshotDocument document, CancellationToken ct = default);
        //Task WriteCrowdSnapshotAsync(CrowdSnapshotDocument snapshot, CancellationToken ct = default);
    }
}


































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.