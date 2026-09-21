using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using EconomicTrendsPolLESSIRE.Infrastructure.Repositories;
using EconomicTrendsPolLESSIRE.Shared.StaticConfig.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using static EconomicTrendsPolLESSIRE.Application.Extensions.MapperExtensions;
using HubEvents = EconomicTrendsPolLESSIRE.Contracts.Hubs.MarketHubMethods;


namespace EconomicTrendsPolLESSIRE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketSnapshotController : ControllerBase
    {
        private readonly IMarketSnapshotRepository _marketSnapshotRepository;
        private readonly IHubContext<MarketDataHub> _hubContext;

        private const string HubMethod_ReceiveEventUpdate = "ReceiveEventUpdate";

        public MarketSnapshotController(IMarketSnapshotRepository markerSnapshotRepository, IHubContext<MarketDataHub> hubContext)
        {
            _marketSnapshotRepository = markerSnapshotRepository;
            _hubContext = hubContext;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<MarketSnapshotDTO>>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            var marketSnapshots = await _marketSnapshotRepository.GetAllMarketSnapshotAsync(limit, ct);
            var result = marketSnapshots.Select(x => x.MapToMarketSnapshotDTO()).ToList();

            return Ok(result);
        }

        [HttpGet("{instrumentId:long}")]
        public async Task<ActionResult<MarketSnapshotDTO>> GetMarketSnapshotByIdAsync(long instrumentId, CancellationToken ct = default)
        {
            if (instrumentId <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var marketSnapshot = await _marketSnapshotRepository.GetMarketSnapshotByIdAsync(instrumentId, ct);

            if (marketSnapshot == null)
            {
                return NotFound($"No market snapshot found for the identifier {instrumentId}.");
            }

            return Ok(marketSnapshot.MapToMarketSnapshotDTO());
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpPost]
        public async Task<ActionResult<MarketSnapshotDTO>> SaveMarketSnapshotAsync([FromBody] MarketSnapshotDTO dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = dto.MapToMarketSnapshot();
            var saved = await _marketSnapshotRepository.SaveMarketSnapshotAsync(entity);

            if (saved == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to save the market snapshot.");
            }

            var result = saved.MapToMarketSnapshotDTO();

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketSnapshotUpdated, result, ct);

            return Ok(result);
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpDelete("archive/{instrumentId:long}")]
        public async Task<IActionResult> DeleteMarketSnapshotAsync(long instrumentId, CancellationToken ct = default)
        {
            if (instrumentId <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var deleted = await _marketSnapshotRepository.DeleteMarketSnapshotAsync(instrumentId);

            if (!deleted)
            {
                return NotFound($"No active market snapshot found for the identifier {instrumentId}.");
            }

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketSnapshotArchived, instrumentId, ct);

            return Ok(new
            {
                Message = $"market snapshot with ID {instrumentId} successfully archived."
            });
        }
    }
}




















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.