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
    public class ProviderInstrumentController : ControllerBase
    {
        private readonly IProviderInstrumentRepository _providerInstrumentRepository;
        private readonly IHubContext<MarketDataHub> _hubContext;

        private const string HubMethod_ReceiveEventUpdate = "ReceiveEventUpdate";

        public ProviderInstrumentController(IProviderInstrumentRepository providerInstrumentRepository, IHubContext<MarketDataHub> hubContext)
        {
            _providerInstrumentRepository = providerInstrumentRepository;
            _hubContext = hubContext;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ProviderInstrumentDTO>>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            var providerInstruments = await _providerInstrumentRepository.GetAllProviderInstrumentAsync(limit, ct);
            var result = providerInstruments.Select(x => x.MapToProviderInstrumentDTO()).ToList();

            return Ok(result);
        }

        [HttpGet("{providerId:int}/{instrumentId:long}")]
        public async Task<ActionResult<ProviderInstrumentDTO>> GetProviderInstrumentByIdAsync(int providerId, long instrumentId, CancellationToken ct = default)
        {
            if (providerId <= 0 || instrumentId <= 0)
            {
                return BadRequest("The provider ID and instrument ID must be greater than zero.");
            }

            var providerInstrument = await _providerInstrumentRepository.GetProviderInstrumentByIdAsync(providerId, instrumentId, ct);

            if (providerInstrument == null)
            {
                return NotFound();
            }

            return Ok(providerInstrument.MapToProviderInstrumentDTO());
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpPost]
        public async Task<ActionResult<ProviderInstrumentDTO>> SaveProviderInstrumentAsync([FromBody] ProviderInstrumentDTO dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = dto.MapToProviderInstrument();
            var saved = await _providerInstrumentRepository.SaveProviderInstrumentAsync(entity);

            if (saved == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to save the provider instrument.");
            }

            var result = saved.MapToProviderInstrumentDTO();

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.ProviderInstrumentUpdated, result, ct);

            return Ok(result);
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpDelete("archive/{providerId:int}/{instrumentId:long}")]
        public async Task<IActionResult> DeleteProviderInstrumentAsync(int providerId, long instrumentId, CancellationToken ct = default)
        {
            var deleted = await _providerInstrumentRepository.DeleteProviderInstrumentAsync(providerId, instrumentId);

            if (!deleted)
            {
                return NotFound();
            }

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.ProviderInstrumentArchived, new { ProviderId = providerId,  InstrumentId = instrumentId }, ct);

            return Ok();
        }
    }
}
















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.