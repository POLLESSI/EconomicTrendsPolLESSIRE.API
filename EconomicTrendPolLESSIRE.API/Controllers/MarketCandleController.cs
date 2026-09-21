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
using System.Diagnostics.Metrics;
using static EconomicTrendsPolLESSIRE.Application.Extensions.MapperExtensions;
using HubEvents = EconomicTrendsPolLESSIRE.Contracts.Hubs.MarketHubMethods;

namespace EconomicTrendsPolLESSIRE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketCandleController : ControllerBase
    {
        private readonly IMarketCandleRepository _marketCandleRepository;
        private readonly IHubContext<MarketDataHub> _hubContext;

        private const string HubMethod_ReceiveEventUpdate = "ReceiveEventUpdate";

        public MarketCandleController(IMarketCandleRepository markerCandleRepository, IHubContext<MarketDataHub> hubContext)
        {
            _marketCandleRepository = markerCandleRepository;
            _hubContext = hubContext;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<MarketCandleDTO>>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            var instruments = await _marketCandleRepository.GetAllMarketCandleAsync(limit, ct);
            var result = instruments.Select(x => x.MapToMarketCandleDTO()).ToList();

            return Ok(result);
        }

        [HttpGet("instrument/{instrumentId:long}")]
        public async Task<ActionResult<MarketCandleDTO>>GetMarketCandleByIdAsync(long instrumentId, CancellationToken ct = default)
        {
            if (instrumentId <= 0)
            {
                return BadRequest("The provided instrument ID is invalid.");
            }

            var marketCandle = await _marketCandleRepository.GetMarketCandleByIdAsync(instrumentId, ct);

            if (marketCandle == null)
            {
                return NotFound($"No market candle found for instrument {instrumentId}.");
            }

            return Ok(marketCandle.MapToMarketCandleDTO());
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpPost]
        public async Task<ActionResult<MarketCandleDTO>> SaveMarketCandleAsync([FromBody] MarketCandleDTO dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = dto.MapToMarketCandle();
            var saved = await _marketCandleRepository.SaveMarketCandleAsync(entity);

            if (saved == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to save the market candle.");
            }

            var result = saved.MapToMarketCandleDTO();

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketCandleUpdated, result, ct);

            return Ok(result);
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpDelete("archive/instrument/{instrumentId:long}")]
        public async Task<IActionResult> DeleteMarketCandleAsync(long instrumentId, CancellationToken ct = default)
        {
            if (instrumentId <= 0)
            {
                return BadRequest("The provided instrument ID is invalid.");
            }

            var deleted = await _marketCandleRepository.DeleteMarketCandleAsync(instrumentId);

            if (!deleted)
            {
                return NotFound($"No active market candle found for instrument {instrumentId}.");
            }

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketCandleArchived, instrumentId, ct);

            return Ok(new
            {
                Message = $"Market candle(s) for instrument {instrumentId} successfully archived."
            });
        }
    }
}





















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.