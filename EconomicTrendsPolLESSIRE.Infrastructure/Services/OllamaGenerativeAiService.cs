using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public sealed class OllamaGenerativeAiService : IGenerativeAiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IMongoSnapshotWriter _mongoSnapshotWriter;
        private readonly ILogger<OllamaGenerativeAiService> _logger;

        public OllamaGenerativeAiService(HttpClient httpClient, IConfiguration config, IMongoSnapshotWriter mongoSnapshotWriter, ILogger<OllamaGenerativeAiService> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _mongoSnapshotWriter = mongoSnapshotWriter;
            _logger = logger;
        }

        public async Task<string> GenerateTextAsync(string prompt, CancellationToken ct = default)
        {
            var model = _config["MistralAI:Model"] ?? "mistral";
            var temperature = _config.GetValue<float?>("MistralAI:Temperature") ?? 0.3f;

            var request = new
            {
                model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                stream = false,
                keep_alive = "30m",
                options = new
                {
                    temperature,
                    num_predict = 180,
                    num_ctx = 2048
                }
            };

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "api/chat")
            {
                Content = JsonContent.Create(request)
            };

            var sw = Stopwatch.StartNew();

            using var response = await _httpClient.SendAsync(httpRequest, ct);

            _logger.LogInformation("Ollama answered in {Elapsed} ms", sw.ElapsedMilliseconds);

            var content = await response.Content.ReadAsStringAsync(ct);

            _logger.LogInformation("Ollama status code: {StatusCode}", (int)response.StatusCode);

            response.EnsureSuccessStatusCode();

            var json = JsonSerializer.Deserialize<MistralResponse>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _logger.LogInformation("[OLLAMA] Response parsed after {Elapsed} ms", sw.ElapsedMilliseconds);

            return json?.Message?.Content?.Trim() ?? "No response from Ollama.";
        }
    }
}

















































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.