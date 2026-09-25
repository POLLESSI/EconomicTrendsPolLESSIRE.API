using Azure;
using Azure.Core;
using EconomicTrendsPolLESSIRE.Application.Extensions;
using EconomicTrendsPolLESSIRE.Application.Mistral;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Hubs;
using EconomicTrendsPolLESSIRE.Domain.DTOs;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.DTOs.DTOs;
using EconomicTrendsPolLESSIRE.Hubs.Extensions;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using EconomicTrendsPolLESSIRE.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public sealed class MistralOrchestrator : IMistralOrchestrator, IMistralQueuedRequestProcessor
    {
        private const int MaxEconomicTrendRecommendations = 20;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<MistralHub, IMistralClient> _hubContext;
        private readonly IMistralRequestRegistry _mistralRequestRegistry;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<MistralOrchestrator> _logger;
        private readonly EconomicTrendsDomainGuard _domainGuard;
        private readonly IMistralBackgroundQueue _backgroundQueue;

        public MistralOrchestrator(IServiceScopeFactory scopeFactory, IHubContext<MistralHub, IMistralClient> hubContext, IMistralRequestRegistry mistralRequestRegistry, IHostApplicationLifetime appLifetime, IMistralBackgroundQueue backgroundQueue, ILogger<MistralOrchestrator> logger, EconomicTrendsDomainGuard domainGuard)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _mistralRequestRegistry = mistralRequestRegistry ?? throw new ArgumentNullException(nameof(mistralRequestRegistry));
            _appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domainGuard = domainGuard ?? throw new ArgumentNullException(nameof(domainGuard));
            _backgroundQueue = backgroundQueue ?? throw new ArgumentNullException(nameof(backgroundQueue));

            _logger.LogInformation("[Mistral ECONOMIC] EconomicTrends orchestrator loaded.");
        }
        public async Task<MistralStartResponseDto> StartMistralRequestAsync(MistralPromptRequest request, CancellationToken ct = default)
        {
            ValidateRequest(request);

            var prompt = request.Prompt.Trim();
            var interaction = await CreateInitialInteractionAsync(request, prompt, ct).ConfigureAwait(false);
            /*
            * The background processing must survive
            * at the end of the HTTP request.
            *
            * However, the application stops running
            * capable of cancelling it.
            */
            var processingCts = CancellationTokenSource.CreateLinkedTokenSource(_appLifetime.ApplicationStopping);
            var requestId = _mistralRequestRegistry.Register(interaction.Id, processingCts);

            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_appLifetime.ApplicationStopping);
            var startedAtUtc = DateTime.UtcNow;

            try
            {
                var workItem = new MistralWorkItem(Interaction: interaction, Request: request, RequestId: requestId, ProcessingToken: processingCts.Token);
                await _backgroundQueue.QueueAsync(workItem, ct);
            }
            catch
            {
                _mistralRequestRegistry.Remove(interaction.Id, requestId);
                throw;
            }

            _logger.LogInformation("[Mistral-QUEUE] Accepted. " + "InteractionId={InteractionId}, " + "RequestId={RequestId}, " + "PromptLength={PromptLength}", interaction.Id, requestId, prompt.Length);

            return new MistralStartResponseDto
            {
                Accepted = true,
                InteractionId = interaction.Id,
                RequestId = requestId,
                StartedAtUtc = DateTime.UtcNow,
                Status = "accepted",
                Message = "Mistral request accepted and queued."
            };
        }
        public async Task<MistralInteractionDTO> RunMistralRequestAsync(MistralPromptRequest request, CancellationToken ct = default)
        {
            ValidateRequest(request);

            var prompt = request.Prompt.Trim();

            _logger.LogInformation(
                "[Mistral-PIPELINE][SYNC] Started. PromptLength={PromptLength}, Lat={Lat}, Lng={Lng}, HttpTokenCanBeCanceled={HttpTokenCanBeCanceled}, HttpTokenIsCanceled={HttpTokenIsCanceled}",
                prompt.Length,
                request.Latitude,
                request.Longitude,
                ct.CanBeCanceled,
                ct.IsCancellationRequested);

            var interaction = await CreateInitialInteractionAsync(request, prompt, ct).ConfigureAwait(false);
            var interactionId = interaction.Id;

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, _appLifetime.ApplicationStopping);
            var requestId = _mistralRequestRegistry.Register(interactionId, linkedCts);

            try
            {
                await _hubContext.SendStarted(
                    new MistralResponseStartedDto
                    {
                        InteractionId = interactionId,
                        RequestId = requestId,
                        StartedAtUtc = DateTime.UtcNow
                    });

                var finalDto = await ExecutePipelineInternalAsync(
                    request: request,
                    prompt: prompt,
                    interactionId: interactionId,
                    requestId: requestId,
                    ct: linkedCts.Token,
                    pushChunksToHub: false,
                    emitStartedEvent: false).ConfigureAwait(false);

                await _hubContext.SendCompleted(ToCompletedDto(finalDto));

                await _hubContext.SendStatus(
                    new MistralResponseStatusDto
                    {
                        InteractionId = interactionId,
                        RequestId = requestId,
                        Status = "completed",
                        Message = "Generation completed.",
                        TimestampUtc = DateTime.UtcNow
                    });

                return finalDto;
            }

            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(
                    ex,
                    "[Mistral-PIPELINE][SYNC] Cancelled. InteractionId={InteractionId}, RequestId={RequestId}",
                    interactionId,
                    requestId);

                await MarkCancelledSafeAsync(interactionId, "Generation cancelled.", CancellationToken.None);

                await _hubContext.SendStatus(
                    new MistralResponseStatusDto
                    {
                        InteractionId = interactionId,
                        RequestId = requestId,
                        Status = "cancelled",
                        Message = "Generation cancelled.",
                        TimestampUtc = DateTime.UtcNow
                    });

                throw;
            }

            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "[Mistral-PIPELINE][SYNC] Failed. InteractionId={InteractionId}, RequestId={RequestId}",
                    interactionId,
                    requestId);

                await MarkFailedSafeAsync(interactionId, ex.Message).ConfigureAwait(false);

                await _hubContext.SendStatus(
                    new MistralResponseStatusDto
                    {
                        InteractionId = interactionId,
                        RequestId = requestId,
                        Status = "failed",
                        Message = ex.Message,
                        TimestampUtc = DateTime.UtcNow
                    });

                throw;
            }
            finally
            {
                _mistralRequestRegistry.Remove(interactionId, requestId);

                _logger.LogInformation("[Mistral-PIPELINE][SYNC] Cleanup done. InteractionId={InteractionId}, RequestId={RequestId}", interactionId, requestId);
            }
        }

        public Task<bool> CancelAsync(int interactionId, string? requestId = null)
        {
            var cancelled = _mistralRequestRegistry.TryCancel(interactionId, requestId);

            _logger.LogInformation("[Mistral-PIPELINE] Cancel requested. InteractionId={InteractionId}, RequestId={RequestId}, Cancelled={Cancelled}", interactionId, requestId, cancelled);

            return Task.FromResult(cancelled);
        }

        public async Task ProcessQueuedAsync(MistralWorkItem workItem, CancellationToken stoppingToken)
        {
            ArgumentNullException.ThrowIfNull(workItem);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(workItem.ProcessingToken, stoppingToken);

            await RunPipelineAsync(workItem.Interaction, workItem.Request, workItem.RequestId, linkedCts.Token).ConfigureAwait(false);
        }

        private async Task RunPipelineAsync(MistralInteraction interaction, MistralPromptRequest request, string requestId, CancellationToken ct)
        {
            try
            {
                await _hubContext.SendStarted(
                    new MistralResponseStartedDto
                    {
                        InteractionId = interaction.Id,
                        RequestId = requestId,
                        StartedAtUtc = DateTime.UtcNow
                    });

                var finalDto = await ExecutePipelineInternalAsync(
                    request: request,
                    prompt: request.Prompt.Trim(),
                    interactionId: interaction.Id,
                    requestId: requestId,
                    ct: ct,
                    pushChunksToHub: true,
                    emitStartedEvent: false)
                    .ConfigureAwait(false);

                await _hubContext.SendCompleted(
                    ToCompletedDto(finalDto));

                await _hubContext.SendStatus(
                    new MistralResponseStatusDto
                    {
                        InteractionId = interaction.Id,
                        RequestId = requestId,
                        Status = "completed",
                        Message = "Generation completed.",
                        TimestampUtc = DateTime.UtcNow
                    });
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(
                    ex,
                    "[Mistral-PIPELINE][ASYNC] Cancelled. InteractionId={InteractionId}, RequestId={RequestId}, TokenCanBeCanceled={CanBeCanceled}, TokenIsCancellationRequested={IsCancellationRequested}",
                    interaction.Id,
                    requestId,
                    ct.CanBeCanceled,
                    ct.IsCancellationRequested);

                await MarkCancelledSafeAsync(interaction.Id, "Generation cancelled.", CancellationToken.None);

                await _hubContext.SendStatus(
                    new MistralResponseStatusDto
                    {
                        InteractionId = interaction.Id,
                        RequestId = requestId,
                        Status = "cancelled",
                        Message = "Generation cancelled.",
                        TimestampUtc = DateTime.UtcNow
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "[Mistral-PIPELINE][ASYNC] Failed. InteractionId={InteractionId}, RequestId={RequestId}",
                    interaction.Id,
                    requestId);

                await MarkFailedSafeAsync(interaction.Id, ex.Message).ConfigureAwait(false);

                await _hubContext.SendStatus(
                    new MistralResponseStatusDto
                    {
                        InteractionId = interaction.Id,
                        RequestId = requestId,
                        Status = "failed",
                        Message = ex.Message,
                        TimestampUtc = DateTime.UtcNow
                    });
            }
            finally
            {
                _mistralRequestRegistry.Remove(interaction.Id, requestId);
            }
        }

        private async Task<MistralInteractionDTO> CompleteWithGuardMessageAsync(IMistralInteractionRepository mistralRepository, int interactionId, string requestId, string message, bool pushChunksToHub, CancellationToken ct)
        {
            var updated = await mistralRepository.CompleteAsync(interactionId, message, "DomainGuard", ct).ConfigureAwait(false);

            if (!updated)
                throw new InvalidOperationException($"Failed to persist guarded Mistral response for interaction {interactionId}.");

            var persisted = await mistralRepository.GetByIdAsync(interactionId)
                .ConfigureAwait(false);

            if (persisted is null)
                throw new InvalidOperationException($"Mistral interaction {interactionId} not found after guard response.");

            if (pushChunksToHub)
            {
                await _hubContext.SendChunk(
                    new MistralResponseChunkDto
                    {
                        InteractionId = interactionId,
                        RequestId = requestId,
                        Chunk = message,
                        IsFinal = false
                    });

                await _hubContext.SendChunk(
                    new MistralResponseChunkDto
                    {
                        InteractionId = interactionId,
                        RequestId = requestId,
                        Chunk = string.Empty,
                        IsFinal = true
                    });
            }

            _logger.LogWarning("[Mistral DOMAIN GUARD] Request blocked. InteractionId={InteractionId}, RequestId={RequestId}", interactionId, requestId);

            return persisted.MapToMistralInteractionDTO();
        }
        private static void ValidateRequest(MistralPromptRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Prompt))
                throw new ArgumentException("Prompt cannot be empty.", nameof(request));
        }

        private async Task<MistralInteraction> CreateInitialInteractionAsync(MistralPromptRequest request, string prompt, CancellationToken ct)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var mistralRepository = scope.ServiceProvider.GetRequiredService<IMistralInteractionRepository>();

            var interaction = new MistralInteraction
            {
                Prompt = prompt,
                Response = string.Empty,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                Model = "mistral",
                Temperature = 0.3f,
                ExecutionSource = "MistralLocal",
                Status = "Pending",
                SourceType = null,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            var created = await mistralRepository.CreatePendingAsync(interaction, ct).ConfigureAwait(false);

            if (created is null || created.Id <= 0)
                throw new InvalidOperationException("Unable to create Mistral interaction.");

            _logger.LogInformation(
                "[Mistral-PIPELINE] Initial interaction persisted. InteractionId={InteractionId}, PromptHash={PromptHash}, CreatedAt={CreatedAt}",
                created.Id,
                created.PromptHash,
                created.CreatedAt);

            return created;
        }

        private async Task<MistralInteractionDTO> ExecutePipelineInternalAsync(MistralPromptRequest request, string prompt, int interactionId, string requestId, CancellationToken ct, bool pushChunksToHub, bool emitStartedEvent)
        {
            var sw = Stopwatch.StartNew();

            if (emitStartedEvent)
            {
                await _hubContext.SendStarted(
                    new MistralResponseStartedDto
                    {
                        InteractionId = interactionId,
                        RequestId = requestId,
                        StartedAtUtc = DateTime.UtcNow
                    });
            }

            await using var scope = _scopeFactory.CreateAsyncScope();

            var mistralRepository = scope.ServiceProvider.GetRequiredService<IMistralInteractionRepository>();
            var localAiContextService = scope.ServiceProvider.GetRequiredService<ILocalAiContextService>();
            var mistralAiService = scope.ServiceProvider.GetRequiredService<IMistralAIService>();

            // =========================================================
            // DOMAIN GUARD
            // =========================================================

            var inputGuard = _domainGuard.CheckInput(prompt);

            if (!inputGuard.Allowed)
            {
                var blockedResponse = inputGuard.Message ?? "This request is outside the supported EconomicTrends domain.";

                return await CompleteWithGuardMessageAsync( mistralRepository, interactionId, requestId, blockedResponse, pushChunksToHub, ct).ConfigureAwait(false);
            }

            // =========================================================
            // LANGUAGE
            // =========================================================

            var responseLanguage = ResolveResponseLanguage(prompt, request.LanguageCode);

            /*
             * Do not translate the economic question here.
             *
             * The original meaning must remain intact.
             * We only normalize whitespace.
             */
            var contextPrompt = NormalizeEconomicPromptForContext(prompt);

            _logger.LogInformation("[Mistral ECONOMIC] " + "OriginalPrompt={OriginalPrompt}; " + "ContextPrompt={ContextPrompt}; " + "Language={Language}", prompt, contextPrompt, responseLanguage);

            // =========================================================
            // OPTIONAL COORDINATES
            // =========================================================

            double? effectiveLatitude = null;
            double? effectiveLongitude = null;

            if (HasValidCoordinates(request.Latitude, request.Longitude))
            {
                effectiveLatitude = request.Latitude!.Value;
                effectiveLongitude = request.Longitude!.Value;

                /*
                 * Coordinates remain optional metadata.
                 *
                 * We no longer resolve a Place,
                 * city or tourist origin.
                 */
                var locationUpdated = await mistralRepository.UpdateLocationAsync(interactionId, effectiveLatitude.Value, effectiveLongitude.Value, ct).ConfigureAwait(false);

                if (!locationUpdated)
                {
                    _logger.LogWarning(
                        "[Mistral ECONOMIC] " +
                        "Unable to persist optional coordinates. " +
                        "InteractionId={InteractionId}",
                        interactionId);
                }
            }

            // =========================================================
            // ECONOMIC CONTEXT
            // =========================================================

            var swContext = Stopwatch.StartNew();
            var localContext = await localAiContextService.BuildContextAsync(contextPrompt, effectiveLatitude, effectiveLongitude, ct).ConfigureAwait(false);

            swContext.Stop();

            _logger.LogInformation(
                "[Mistral-PIPELINE] Economic context built. " +
                "InteractionId={InteractionId}; " +
                "RequestId={RequestId}; " +
                "ElapsedMs={ElapsedMs}; " +
                "Instruments={Instruments}; " +
                "KeywordInstruments={KeywordInstruments}; " +
                "Candles={Candles}; " +
                "Quotes={Quotes}; " +
                "Snapshots={Snapshots}; " +
                "Trades={Trades}; " +
                "Providers={Providers}; " +
                "ProviderInstruments={ProviderInstruments}; " +
                "Indicators={Indicators}; " +
                "UserMessages={UserMessages}",
                interactionId,
                requestId,
                swContext.ElapsedMilliseconds,
                localContext.Instruments.Count,
                localContext.KeywordMatchedInstruments.Count,
                localContext.MarketCandles.Count,
                localContext.MarketQuotes.Count,
                localContext.MarketSnapshots.Count,
                localContext.MarketTrades.Count,
                localContext.Providers.Count,
                localContext.ProviderInstruments.Count,
                localContext.TechnicalIndicators.Count,
                localContext.UserMessages.Count);

            // =========================================================
            // GROUNDED PROMPT
            // =========================================================

            var groundedPrompt = localAiContextService.BuildPrompt(localContext);

            groundedPrompt +=
                $"""

        ORIGINAL USER QUESTION
        {prompt}

        RESPONSE RULES:
        - Answer in the language of the original user question.
        - Use only the verified economic and market context supplied above.
        - Do not invent instruments, symbols, prices, volumes, providers,
          quotes, trades, candles or technical indicators.
        - Distinguish observed data from interpretation.
        - If the available data is insufficient, state this explicitly.
        - Do not present an interpretation as a guaranteed future outcome.
        - Prefer the most recent supplied market information when timestamps differ.
        """;

            _logger.LogInformation(
                "[Mistral-PIPELINE] Grounded economic prompt built. " +
                "InteractionId={InteractionId}; " +
                "RequestId={RequestId}; " +
                "Characters={Characters}",
                interactionId,
                requestId,
                groundedPrompt.Length);

            // =========================================================
            // GENERATION
            // =========================================================

            string finalResponse;
            var finalSourceType = "MistralLocal";

            try
            {
                if (pushChunksToHub)
                {
                    var approximatePromptTokens = (int)Math.Ceiling(groundedPrompt.Length / 4d);

                    _logger.LogInformation(
                        "[Mistral FINAL PROMPT SIZE] " +
                        "InteractionId={InteractionId}; " +
                        "Characters={Characters}; " +
                        "ApproximateTokens={ApproximateTokens}",
                        interactionId,
                        groundedPrompt.Length,
                        approximatePromptTokens);

                    finalResponse =
                        await mistralAiService.StreamFromPromptAsync(groundedPrompt,
                                async chunkText =>
                                {
                                    if (string.IsNullOrEmpty(chunkText))
                                    {
                                        return;
                                    }

                                    await _hubContext.SendChunk(
                                        new MistralResponseChunkDto
                                        {
                                            InteractionId = interactionId,
                                            RequestId = requestId,
                                            Chunk = chunkText,
                                            IsFinal = false
                                        });
                                },
                                responseLanguage: responseLanguage, ct: ct).ConfigureAwait(false);
                }
                else
                {
                    finalResponse = await mistralAiService.GenerateFromPromptAsync(groundedPrompt: groundedPrompt, responseLanguage: responseLanguage, ct: ct).ConfigureAwait(false);
                }
            }
            catch (TimeoutException ex)
            {
                finalSourceType = "EconomicTrendFallbackTimeout";

                var verifiedInstruments = GetVerifiedEconomicTrendCandidates(localContext);

                finalResponse = BuildVerifiedEconomicTrendResponse(verifiedInstruments, responseLanguage);

                _logger.LogWarning(
                    ex,
                    "[Mistral OLLAMA FALLBACK] " +
                    "Timeout. " +
                    "InteractionId={InteractionId}; " +
                    "VerifiedInstruments={VerifiedInstruments}; " +
                    "FallbackLength={FallbackLength}",
                    interactionId,
                    verifiedInstruments.Count,
                    finalResponse.Length);

                if (pushChunksToHub && !string.IsNullOrWhiteSpace(finalResponse))
                {
                    await _hubContext.SendChunk(
                        new MistralResponseChunkDto
                        {
                            InteractionId = interactionId,
                            RequestId = requestId,
                            Chunk = finalResponse,
                            IsFinal = false
                        });
                }
            }

            // =========================================================
            // EMPTY RESPONSE FALLBACK
            // =========================================================

            if (string.IsNullOrWhiteSpace( finalResponse))
            {
                finalResponse = BuildNoEconomicDataMessage(responseLanguage);
            }

            _logger.LogInformation(
                "[Mistral-PIPELINE] Generation finished. " +
                "InteractionId={InteractionId}; " +
                "RequestId={RequestId}; " +
                "ResponseLength={ResponseLength}",
                interactionId,
                requestId,
                finalResponse.Length);

            // =========================================================
            // OUTPUT GUARD
            // =========================================================

            var outputGuard = _domainGuard.CheckOutput(finalResponse);

            if (!outputGuard.Allowed)
            {
                finalResponse = outputGuard.Message ?? "The generated response was blocked because it falls outside the EconomicTrends domain.";
            }

            // =========================================================
            // PERSISTENCE
            // =========================================================

            var updated =
                await mistralRepository
                    .CompleteAsync(
                        interactionId,
                        finalResponse,
                        finalSourceType,
                        ct)
                    .ConfigureAwait(false);

            if (!updated)
            {
                throw new InvalidOperationException(
                    $"Failed to persist final Mistral response " +
                    $"for interaction {interactionId}.");
            }

            var persisted =
                await mistralRepository
                    .GetByIdAsync(interactionId)
                    .ConfigureAwait(false);

            if (persisted is null)
            {
                throw new InvalidOperationException(
                    $"Mistral interaction {interactionId} " +
                    "not found after update.");
            }

            var finalDto = persisted.MapToMistralInteractionDTO();

            if (pushChunksToHub)
            {
                await _hubContext.SendChunk(
                    new MistralResponseChunkDto
                    {
                        InteractionId = interactionId,
                        RequestId = requestId,
                        Chunk = string.Empty,
                        IsFinal = true
                    });
            }

            sw.Stop();

            _logger.LogInformation(
                "[Mistral-PIPELINE] Final interaction persisted. " +
                "InteractionId={InteractionId}; " +
                "RequestId={RequestId}; " +
                "TotalElapsedMs={ElapsedMs}; " +
                "PersistedResponseLength={PersistedResponseLength}",
                finalDto.Id,
                requestId,
                sw.ElapsedMilliseconds,
                finalDto.Response?.Length ?? 0);

            return finalDto;
        }

        private static string NormalizeFactKey(string value)
        {
            var decomposed = value.Normalize(NormalizationForm.FormD);

            var builder = new StringBuilder(decomposed.Length);

            foreach (var character in decomposed)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(character);

                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(char.ToLowerInvariant(character));
                }
            }

            return Regex.Replace(builder.ToString(), @"[^\p{L}\p{N}]+", " ").Trim();
        }

        private async Task MarkFailedSafeAsync(int interactionId, string? errorMessage)
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var mistralRepository = scope.ServiceProvider.GetRequiredService<IMistralInteractionRepository>();

                await mistralRepository.MarkFailedAsync(interactionId, errorMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "[Mistral-PIPELINE] Failed to mark interaction as failed. InteractionId={InteractionId}",
                    interactionId);
            }
        }

        private static MistralInteractionCompletedDto ToCompletedDto(MistralInteractionDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new MistralInteractionCompletedDto
            {
                Id = dto.Id,
                Prompt = dto.Prompt ?? string.Empty,
                Response = dto.Response ?? string.Empty,
                PromptHash = dto.PromptHash,
                CreatedAt = dto.CreatedAt,
                Active = dto.Active,

                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                SourceType = dto.SourceType
            };
        }

        private static bool HasValidCoordinates(double? latitude, double? longitude)
        {
            return
                latitude.HasValue &&
                longitude.HasValue &&
                double.IsFinite(latitude.Value) &&
                double.IsFinite(longitude.Value) &&
                latitude.Value is >= -90d and <= 90d &&
                longitude.Value is >= -180d and <= 180d &&
                !(latitude.Value == 0d &&
                  longitude.Value == 0d);
        }
        
        private static string ResolveResponseLanguage(string prompt, string? requestedLanguage)
        {
            if (!string.IsNullOrWhiteSpace(prompt))
            {
                // Cyrillic alphabet.
                if (Regex.IsMatch(prompt, @"[\u0400-\u04FF]"))
                {
                    return "ru-RU";
                }

                // Chinese characters.
                if (Regex.IsMatch(prompt, @"[\u4E00-\u9FFF]"))
                {
                    return "zh-CN";
                }

                // Arabic alphabet.
                if (Regex.IsMatch(prompt, @"[\u0600-\u06FF]"))
                {
                    return "ar";
                }
            }

            return string.IsNullOrWhiteSpace(requestedLanguage) ? "fr-FR" : requestedLanguage.Trim();
        }

        private static string NormalizeEconomicPromptForContext(
    string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                return string.Empty;
            }

            return Regex.Replace(
                    prompt.Trim(),
                    @"\s+",
                    " ")
                .Trim();
        }
        private async Task MarkCancelledSafeAsync(int interactionId, string? message, CancellationToken ct = default)
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();

                var repository =
                    scope.ServiceProvider
                        .GetRequiredService<IMistralInteractionRepository>();

                await repository.MarkCancelledAsync(
                        interactionId,
                        message,
                        ct)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
                when (ct.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "[Mistral-PIPELINE] MarkCancelledSafeAsync cancelled. InteractionId={InteractionId}",
                    interactionId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "[Mistral-PIPELINE] Failed to mark interaction as cancelled. InteractionId={InteractionId}",
                    interactionId);
            }
        }
        private sealed record VerifiedEconomicTrendCandidate(long Id, string Symbol, string Name, int AssetClass, string? ExchangeCode, string? CurrencyCode);

        private static IReadOnlyList<VerifiedEconomicTrendCandidate>GetVerifiedEconomicTrendCandidates(LocalAiContextDTO context)
        {
            ArgumentNullException.ThrowIfNull(context);

            return context.Instruments
                .Concat(context.KeywordMatchedInstruments)
                .Where(instrument =>
                    instrument.Active &&
                    (
                        !string.IsNullOrWhiteSpace(instrument.Symbol) ||
                        !string.IsNullOrWhiteSpace(instrument.Name)
                    ))
                .GroupBy(instrument => instrument.Id > 0 ? $"ID:{instrument.Id}" : $"SYMBOL:{instrument.Symbol?.Trim()}:{instrument.ExchangeCode?.Trim()}", StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .Select(instrument =>
                    new VerifiedEconomicTrendCandidate(
                        Id: instrument.Id,
                        Symbol: instrument.Symbol?.Trim() ?? string.Empty,
                        Name: instrument.Name?.Trim() ?? string.Empty,
                        AssetClass: instrument.AssetClass,
                        ExchangeCode: instrument.ExchangeCode?.Trim(),
                        CurrencyCode: instrument.CurrencyCode?.Trim()))
                .OrderBy(candidate => candidate.Symbol, StringComparer.OrdinalIgnoreCase)
                .ThenBy(candidate => candidate.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
        private static string BuildVerifiedEconomicTrendResponse(IReadOnlyList<VerifiedEconomicTrendCandidate> candidates, string responseLanguage)
        {
            if (candidates.Count == 0)
            {
                return BuildNoEconomicDataMessage(responseLanguage);
            }

            var result = new StringBuilder();

            if (responseLanguage.StartsWith("fr", StringComparison.OrdinalIgnoreCase))
            {
                result.AppendLine(
                    "Le modèle local n’a pas terminé son analyse. " +
                    "Voici les instruments vérifiés disponibles :");
            }
            else if (responseLanguage.StartsWith(
                         "ru",
                         StringComparison.OrdinalIgnoreCase))
            {
                result.AppendLine(
                    "Локальная модель не завершила анализ. " +
                    "Доступны следующие проверенные инструменты:");
            }
            else
            {
                result.AppendLine(
                    "The local model did not complete its analysis. " +
                    "These verified instruments are available:");
            }

            result.AppendLine();

            foreach (var candidate in candidates.Take(MaxEconomicTrendRecommendations))
            {
                result.Append("- ");

                if (!string.IsNullOrWhiteSpace(candidate.Name))
                {
                    result.Append(candidate.Name);
                }

                if (!string.IsNullOrWhiteSpace(candidate.Symbol))
                {
                    result.Append($" ({candidate.Symbol})");
                }

                result.Append($" — AssetClass={candidate.AssetClass}");

                if (!string.IsNullOrWhiteSpace(candidate.ExchangeCode))
                {
                    result.Append($" — Exchange={candidate.ExchangeCode}");
                }

                if (!string.IsNullOrWhiteSpace(candidate.CurrencyCode))
                {
                    result.Append($" — Currency={candidate.CurrencyCode}");
                }

                result.AppendLine();
            }

            return result
                .ToString()
                .Trim();
        }

        private static string BuildNoEconomicDataMessage(string responseLanguage)
        {
            if (responseLanguage.StartsWith("fr", StringComparison.OrdinalIgnoreCase))
            {
                return
                    "Les données économiques vérifiées disponibles " +
                    "sont insuffisantes pour répondre à cette demande.";
            }

            if (responseLanguage.StartsWith("ru", StringComparison.OrdinalIgnoreCase))
            {
                return
                    "Доступных проверенных экономических данных " +
                    "недостаточно для ответа на этот запрос.";
            }

            return
                "The available verified economic data is insufficient " +
                "to answer this request.";
        }
        private static int? ExtractRequestedRecommendationCount(string? prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return null;

            var normalized = prompt.Trim().ToLowerInvariant();

            // Numbers written as numerals.
            var digitMatch = Regex.Match(normalized, @"\b(?<count>[1-9]|10)\b");

            if (digitMatch.Success && int.TryParse(digitMatch.Groups["count"].Value, out var numericCount))
            {
                return Math.Clamp(numericCount, 1, EconomicTrendsRecommendationPolicy.MaxEconomicTrendRecommendations);
            }

            var wordNumbers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                // French
                ["un"] = 1,
                ["une"] = 1,
                ["deux"] = 2,
                ["trois"] = 3,
                ["quatre"] = 4,
                ["cinq"] = 5,
                ["six"] = 6,
                ["sept"] = 7,
                ["huit"] = 8,

                // English
                ["one"] = 1,
                ["two"] = 2,
                ["three"] = 3,
                ["four"] = 4,
                ["five"] = 5,
                ["six"] = 6,
                ["seven"] = 7,
                ["eight"] = 8,

                // Dutch
                ["een"] = 1,
                ["twee"] = 2,
                ["drie"] = 3,
                ["vier"] = 4,
                ["vijf"] = 5,
                ["zes"] = 6,
                ["zeven"] = 7,
                ["acht"] = 8,

                // German
                ["eins"] = 1,
                ["einen"] = 1,
                ["eine"] = 1,
                ["zwei"] = 2,
                ["drei"] = 3,
                ["vier"] = 4,
                ["fünf"] = 5,
                ["funf"] = 5,
                ["sechs"] = 6,
                ["sieben"] = 7,
                ["acht"] = 8
            };

            foreach (var pair in wordNumbers)
            {
                if (Regex.IsMatch(normalized, $@"\b{Regex.Escape(pair.Key)}\b"))
                {
                    return pair.Value;
                }
            }

            return null;
        }
    }
}






































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.