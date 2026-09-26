#if DEBUG
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Abstractions;
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EconomicTrendsPolLESSIRE.API.Controllers.Diagnostics
{
    [Route("api/diagnostics/mongo/read")]
    [ApiController]
    public sealed class MongoReadDiagnosticsController : ControllerBase
    {
        private readonly IMistralInteractionNoSqlRepository _mistralRepository;

        public MongoReadDiagnosticsController(IMistralInteractionNoSqlRepository mistralRepository)
        {
            _mistralRepository = mistralRepository;
        }

        //private readonly ITrafficSnapshotRepository _trafficRepository;

        [Authorize(Roles = "Admin")]
        [HttpGet("mongo")]
        public async Task<IActionResult> MongoHealth(
        [FromServices] IMongoDbContext context,
        CancellationToken ct)
        {
            var result = await context.Database.RunCommandAsync<MongoDB.Bson.BsonDocument>(
                new MongoDB.Bson.BsonDocument("ping", 1),
                cancellationToken: ct);

            return Ok(new { Ok = true, Result = result.ToString() });
        }

        [HttpGet("mistral")]
        public async Task<IActionResult> GetLatestMistral(CancellationToken ct)
        {
            var items = await _mistralRepository.GetLatestAsync(20, ct);
            return Ok(items);
        }

        //[HttpGet("traffic")]
        //public async Task<IActionResult> GetLatestTraffic(CancellationToken ct)
        //{
        //    var items = await _trafficRepository.GetLatestAsync(20, ct);
        //    return Ok(items);
        //}

#if DEBUG
        [HttpPost("Mistral-test")]
        public async Task<IActionResult> WriteMistralTest(
            [FromServices] IMistralInteractionNoSqlRepository repository,
            CancellationToken ct)
        {
            var document = new MistralInteractionDocument
            {
                SqlInteractionId = null,
                PromptHash = "debug",
                PromptPreview = "Diagnostic Mistral prompt",
                Response = "Diagnostic Mistral response",
                Model = "debug-model",
                Success = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            await repository.InsertAsync(document, ct);

            return Ok(new
            {
                Ok = true,
                Collection = "mistral_interactions",
                InsertedId = document.Id.ToString()
            });
        }
#endif
    }
}
#endif



































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.