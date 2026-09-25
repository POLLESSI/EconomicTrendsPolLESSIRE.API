using EconomicTrendsPolLESSIRE.API.Tools;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Application.Extensions;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Enums;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.DTOs.DTOs;
using EconomicTrendsPolLESSIRE.Shared.StaticConfig.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Diagnostics;
using System.Security.Claims;

namespace EconomicTrendsPolLESSIRE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("per-user")]
    public sealed class MistralController : ControllerBase
    {
        private readonly IMistralInteractionRepository _mistralRepository;
        private readonly IMistralOrchestrator _orchestrator;
        private readonly ILogger<MistralController> _logger;

        public MistralController(IMistralInteractionRepository mistralRepository, IMistralOrchestrator orchestrator, ILogger<MistralController> logger)
        {
            _mistralRepository = mistralRepository;
            _orchestrator = orchestrator;
            _logger = logger;
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var interactions = await _mistralRepository.GetAllInteractionsAsync();
            var dtos = interactions?.Select(x => x.MapToMistralInteractionDTO()).ToList() ?? new();
            return Ok(dtos);
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("The provided ID is invalid.");

            var interaction = await _mistralRepository.GetByIdAsync(id);
            if (interaction == null)
                return NotFound($"Interaction with ID {id} not found.");

            return Ok(interaction.MapToMistralInteractionDTO());
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpGet("status/{id:int}")]
        public async Task<IActionResult> GetStatus(int id)
        {
            if (id <= 0)
                return BadRequest("The provided ID is invalid.");

            var interaction = await _mistralRepository.GetByIdAsync(id);

            if (interaction == null)
                return NotFound();

            return Ok(new
            {
                interaction.Id,
                interaction.Status,
                interaction.Response,
                interaction.CreatedAt
            });
        }

        /// <summary>
        /// Asynchronous mode production/real customer.
        /// Returns 202 immediately, then the pipeline continues in the background.
        /// </summary>
        [Authorize(Policy = Policies.UserPolicy)]
        [HttpPost("ask-mistral")]
        public async Task<IActionResult> AskMistral([FromBody] MistralPromptRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request is null || string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("The prompt cannot be empty.");

            var sw = Stopwatch.StartNew();

            _logger.LogInformation(
                "[Mistral] Request received at {Time}. PromptLength={PromptLength}",
                DateTime.UtcNow,
                request.Prompt.Length);

            var result = await _orchestrator.StartMistralRequestAsync(request, ct);

            _logger.LogInformation(
                "[Mistral] Request accepted after {Elapsed} ms. InteractionId={InteractionId}, RequestId={RequestId}",
                sw.ElapsedMilliseconds,
                result?.InteractionId,
                result?.RequestId);

            return Accepted(result);
        }

        /// <summary>
        /// Special synchronous Swagger/debug mode.
        /// Wait for Ollama and return the final answer directly.
        /// </summary>
        [Authorize(Policy = Policies.UserPolicy)]
        [HttpPost("ask-mistral-sync")]
        public async Task<IActionResult> AskMistralSync([FromBody] MistralPromptRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request is null || string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("The prompt cannot be empty.");

            var result = await _orchestrator.RunMistralRequestAsync(request, ct);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpPost("cancel/{interactionId:int}")]
        public async Task<IActionResult> Cancel(int interactionId, [FromQuery] string? requestId = null)
        {
            if (interactionId <= 0)
                return BadRequest("The provided interaction ID is invalid.");

            try
            {
                var cancelled = await _orchestrator.CancelAsync(interactionId, requestId);

                if (!cancelled)
                {
                    return NotFound(new
                    {
                        message = "No active Mistral request found for this interaction, or the requestId does not match.",
                        interactionId,
                        requestId
                    });
                }

                return Ok(new
                {
                    cancellationRequested = true,
                    interactionId,
                    requestId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling Mistral request. InteractionId={InteractionId}", interactionId);

                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Unable to cancel the Mistral request.");
            }
        }

        [Authorize(Policy = "AdminOrModo")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("The provided ID is invalid.");

            var success = await _mistralRepository.DeactivateInteractionAsync(id);
            if (!success)
                return NotFound($"No active Mistral interaction with ID {id} found.");

            return Ok(new { message = "Interaction deactivated successfully." });
        }

        [HttpPost("archive-expired")]
        [Authorize(Policy = Policies.AdminPolicy)]
        public async Task<IActionResult> ArchiveExpiredMistralInteractions()
        {
            var archived = await _mistralRepository.ArchivePastMistralInteractionsAsync();
            return Ok(new { ArchivedCount = archived });
        }
    }
}




















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.