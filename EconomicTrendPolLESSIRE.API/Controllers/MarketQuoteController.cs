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
    public class MarketQuoteController : ControllerBase
    {
        private readonly IMarketQuoteRepository _marketQuoteRepository;
        private readonly IHubContext<MarketDataHub> _hubContext;

        private const string HubMethod_ReceiveEventUpdate = "ReceiveEventUpdate";

        public MarketQuoteController(IMarketQuoteRepository markerQuoteRepository, IHubContext<MarketDataHub> hubContext)
        {
            _marketQuoteRepository = markerQuoteRepository;
            _hubContext = hubContext;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<MarketQuoteDTO>>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            var marketQuotes = await _marketQuoteRepository.GetAllMarketQuoteAsync(limit, ct);
            var result = marketQuotes.Select(x => x.MapToMarketQuoteDTO()).ToList();

            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MarketQuoteDTO>> GetMarketQuoteByIdAsync(long id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var marketQuote = await _marketQuoteRepository.GetMarketQuoteByIdAsync(id, ct);

            if (marketQuote == null)
            {
                return NotFound($"No market quote found for the identifier {id}.");
            }

            return Ok(marketQuote.MapToMarketQuoteDTO());
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpPost]
        public async Task<ActionResult<MarketQuoteDTO>> SaveMarketQuoteAsync([FromBody] MarketQuoteDTO dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = dto.MapToMarketQuote();
            var saved = await _marketQuoteRepository.SaveMarketQuoteAsync(entity);

            if (saved == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to save the market quote.");
            }

            var result = saved.MapToMarketQuoteDTO();

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketQuoteUpdated, result, ct);

            return Ok(result);
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpDelete("archive/{id:long}")]
        public async Task<IActionResult> DeleteMarketQuoteAsync(long id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var deleted = await _marketQuoteRepository.DeleteMarketQuoteAsync(id);

            if (!deleted)
            {
                return NotFound($"No active market quote found for the identifier {id}.");
            }

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.MarketQuoteArchived, id, ct);

            return Ok(new
            {
                Message = $"Market Quote with ID {id} successfully archived."
            });
        }
    }
}




















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.