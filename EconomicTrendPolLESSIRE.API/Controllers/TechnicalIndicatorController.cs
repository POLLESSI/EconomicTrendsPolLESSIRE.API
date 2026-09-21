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
    public class TechnicalIndicatorController : ControllerBase
    {
        private readonly ITechnicalIndicatorRepository _technicalIndicatorRepository;
        private readonly IHubContext<MarketDataHub> _hubContext;

        private const string HubMethod_ReceiveEventUpdate = "ReceiveEventUpdate";

        public TechnicalIndicatorController(ITechnicalIndicatorRepository technicalIndicatorRepository, IHubContext<MarketDataHub> hubContext)
        {
            _technicalIndicatorRepository = technicalIndicatorRepository;
            _hubContext = hubContext;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TechnicalIndicatorDTO>>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            var technicalIndicators = await _technicalIndicatorRepository.GetAllTechnicalIndicatorAsync(limit, ct);
            var result = technicalIndicators.Select(x => x.MapToTechnicalIndicatorDTO()).ToList();

            return Ok(result);
        }

        [HttpGet("by-key")]
        public async Task<ActionResult<TechnicalIndicatorDTO>> GetTechnicalIndicatorByIdAsync([FromQuery] long instrumentId, [FromQuery] int intervalCode, [FromQuery] int indicatorType, [FromQuery] DateTime timestampUtc, CancellationToken ct = default)
        {
            if (instrumentId <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var technicalIndicator = await _technicalIndicatorRepository.GetTechnicalIndicatorByIdAsync(instrumentId, intervalCode, indicatorType, timestampUtc, ct);

            if (technicalIndicator == null)
            {
                return NotFound();
            }

            return Ok(technicalIndicator.MapToTechnicalIndicatorDTO());
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpPost]
        public async Task<ActionResult<TechnicalIndicatorDTO>> SaveTechnicalIndicatorAsync([FromBody] TechnicalIndicatorDTO dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = dto.MapToTechnicalIndicator();
            var saved = await _technicalIndicatorRepository.SaveTechnicalIndicatorAsync(entity);

            if (saved == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to save the technical indicator.");
            }

            var result = saved.MapToTechnicalIndicatorDTO();

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.TechnicalIndicatorUpdated, result, ct);

            return Ok(result);
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpDelete("archive/{id:long}")]
        public async Task<IActionResult> DeleteTechnicalIndicatorAsync(long instrumentId, int intervalCode, int indicatorType, DateTime timestampUtc, CancellationToken ct = default)
        {
            if (instrumentId <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var deleted = await _technicalIndicatorRepository.DeleteTechnicalIndicatorAsync(instrumentId, intervalCode, indicatorType, timestampUtc, ct);

            if (!deleted)
            {
                return NotFound($"No active technical indicator found for the identifier {instrumentId}.");
            }

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.TechnicalIndicatorArchived, instrumentId, ct);

            return Ok();
        }
    }
}





































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.