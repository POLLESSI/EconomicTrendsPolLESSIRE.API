#if DEBUG
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EconomicTrendsPolLESSIRE.API.Controllers.Diagnostics
{
    [Route("api/diagnostics/ollama")]
    [ApiController]
    public sealed class OllamaDiagnosticsController : ControllerBase
    {
        private readonly IGenerativeAiService _ai;

        public OllamaDiagnosticsController(IGenerativeAiService ai)
        {
            _ai = ai;
        }

        [HttpPost("ping")]
        public async Task<IActionResult> Ping(CancellationToken ct)
        {
            var result = await _ai.GenerateTextAsync(
                "Réponds uniquement par OK.",
                ct);

            return Ok(new
            {
                Ok = true,
                Response = result
            });
        }
    }
}
#endif




































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.