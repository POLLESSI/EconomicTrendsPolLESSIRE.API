using EconomicTrendsPolLESSIRE.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EconomicTrendsPolLESSIRE.API.Controllers
{
    [ApiController]
    [Route("api/market-pipeline")]
    [Authorize(Policy = "Admin")]
    public sealed class MarketPipelineController
        : ControllerBase
    {
        private readonly IMarketReferencePipeline
            _referencePipeline;

        private readonly IMarketIngestionPipeline
            _ingestionPipeline;

        public MarketPipelineController(
            IMarketReferencePipeline referencePipeline,
            IMarketIngestionPipeline ingestionPipeline)
        {
            _referencePipeline = referencePipeline;
            _ingestionPipeline = ingestionPipeline;
        }

        [HttpPost("reference/sync")]
        public async Task<IActionResult> SynchronizeReferenceAsync(
            CancellationToken ct)
        {
            await _referencePipeline
                .SynchronizeAsync(ct);

            return Ok(new
            {
                Success = true,
                Message =
                    "Market reference synchronization completed."
            });
        }

        [HttpPost("ingestion/run-once")]
        public async Task<IActionResult> RunIngestionOnceAsync(
            CancellationToken ct)
        {
            await _ingestionPipeline
                .RunOnceAsync(ct);

            return Ok(new
            {
                Success = true,
                Message =
                    "Market ingestion cycle completed."
            });
        }

        //[HttpPost("ingestion/run-twice")]
        //public async Task<IActionResult> RunIngestionTwiceAsync(CancellationToken ct)
        //{
        //    await _ingestionPipeline.RunOnceAsync(ct);

        //    await _ingestionPipeline.RunOnceAsync(ct);

        //    return Ok(new
        //    {
        //        Success = true,
        //        Message = "Two market ingestion cycles completed."
        //    });
        //}
    }
}