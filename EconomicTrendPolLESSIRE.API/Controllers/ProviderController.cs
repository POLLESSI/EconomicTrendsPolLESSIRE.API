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
    public class ProviderController : ControllerBase
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IHubContext<MarketDataHub> _hubContext;

        private const string HubMethod_ReceiveEventUpdate = "ReceiveEventUpdate";

        public ProviderController(IProviderRepository providerRepository, IHubContext<MarketDataHub> hubContext)
        {
            _providerRepository = providerRepository;
            _hubContext = hubContext;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ProviderDTO>>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            var providers = await _providerRepository.GetAllProviderAsync(limit, ct);
            var result = providers.Select(x => x.MapToProviderDTO()).ToList();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProviderDTO>> GetProviderByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var provider = await _providerRepository.GetProviderByIdAsync(id, ct);

            if (provider == null)
            {
                return NotFound($"No provider found for the identifier {id}.");
            }

            return Ok(provider.MapToProviderDTO());
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpPost]
        public async Task<ActionResult<ProviderDTO>> SaveProviderAsync([FromBody] ProviderDTO dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = dto.MapToProvider();
            var saved = await _providerRepository.SaveProviderAsync(entity);

            if (saved == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to save the provider.");
            }

            var result = saved.MapToProviderDTO();

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.ProviderUpdated, result, ct);

            return Ok(result);
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpDelete("archive/{id:int}")]
        public async Task<IActionResult> DeleteProviderAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var deleted = await _providerRepository.DeleteProviderAsync(id);

            if (!deleted)
            {
                return NotFound($"No active provider found for the identifier {id}.");
            }

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.ProviderArchived, id, ct);

            return Ok(new
            {
                Message = $"Provider with ID {id} successfully archived."
            });
        }
    }
}



















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.