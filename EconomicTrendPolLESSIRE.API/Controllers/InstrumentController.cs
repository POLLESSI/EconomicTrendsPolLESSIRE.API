using EconomicTrendsPolLESSIRE.Application.Extensions;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;

namespace EconomicTrendsPolLESSIRE.API.Controllers
{
    [EnableRateLimiting("per-user")]
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentController : ControllerBase
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IHubContext<MarketDataHub> _hubContext;

        public InstrumentController(IInstrumentRepository instrumentRepository, IHubContext<MarketDataHub> hubContext)
        {
            _instrumentRepository = instrumentRepository;
            _hubContext = hubContext;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<InstrumentDTO>>> GetAllAsync(int limit = 500, CancellationToken ct = default)
        {
            var instruments = await _instrumentRepository.GetAllInstrumentAsync(limit, ct);
            var result = instruments.Select(x => x.MapToInstrumentDTO()).ToList();

            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<InstrumentDTO>> GetInstrumentByIdAsync(long id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var instrument = await _instrumentRepository.GetInstrumentByIdAsync(id, ct);

            if (instrument == null)
            {
                return NotFound($"No instrument found for the identifier {id}.");
            }

            return Ok(instrument.MapToInstrumentDTO());
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpPost]
        public async Task<ActionResult<InstrumentDTO>> SaveInstrumentAsync([FromBody] InstrumentDTO dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = dto.MapToIntrument();
            var saved = await _instrumentRepository.SaveInstrumentAsync(entity);

            if (saved == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to save the instrument.");
            }

            var result = saved.MapToInstrumentDTO();

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.InstrumentUpdated, result, ct);

            return Ok(result);
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpDelete("archive/{id:long}")]
        public async Task<IActionResult> DeleteInstrumentAsync(long id, CancellationToken ct = default)
        {
            if (id <= 0)
            {
                return BadRequest("The provided ID is invalid.");
            }

            var deleted = await _instrumentRepository.DeleteInstrumentAsync(id);

            if (!deleted)
            {
                return NotFound($"No active instrument found for the identifier {id}.");
            }

            await _hubContext.Clients.All.SendAsync(MarketHubMethods.ToClient.InstrumentArchived, id, ct);

            return Ok(new
            {
                Message = $"Instrument with ID {id} successfully archived."
            });
        }
    }
}




















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.