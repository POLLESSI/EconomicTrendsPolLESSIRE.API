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
    public class MarketTradeController : ControllerBase
    {
        private readonly IMarketTradeRepository _marketTradeRepository;
        private readonly IHubContext<MarketDataHub> _hubContext;

        private const string HubMethod_ReceiveEventUpdate = "ReceiveEventUpdate";

        public MarketTradeController(IMarketTradeRepository markerTradeRepository, IHubContext<MarketDataHub> hubContext)
        {
            _marketTradeRepository = markerTradeRepository;
            _hubContext = hubContext;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<MarketTradeDTO>>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            var marketTrades = await _marketTradeRepository.GetAllMarketTradeAsync(limit, ct);
            var result = marketTrades.Select(x => x.MapToMarketTradeDTO()).ToList();

            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MarketTradeDTO>> GetMarketTradeByIdAsync(long id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var marketTrade = await _marketTradeRepository.GetMarketTradeByIdAsync(id, ct);

            if (marketTrade == null)
            {
                return NotFound($"No market trade found for the identifier {id}.");
            }

            return Ok(marketTrade.MapToMarketTradeDTO());
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpPost]
        public async Task<ActionResult<MarketTradeDTO>> SaveMarketTradeAsync([FromBody] MarketTradeDTO dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = dto.MapToMarketTrade();
            var saved = await _marketTradeRepository.SaveMarketTradeAsync(entity);

            if (saved == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to save the market trade.");
            }

            var result = saved.MapToMarketTradeDTO();

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketTradeUpdated, result, ct);

            return Ok(result);
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpDelete("archive/{id:long}")]
        public async Task<IActionResult> DeleteMarketTradeAsync(long id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var deleted = await _marketTradeRepository.DeleteMarketTradeAsync(id);

            if (!deleted)
            {
                return NotFound($"No active market trade found for the identifier {id}.");
            }

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketTradeArchived, id, ct);

            return Ok(new
            {
                Message = $"market trade with ID {id} successfully archived."
            });
        }
    }
}

    


























































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.