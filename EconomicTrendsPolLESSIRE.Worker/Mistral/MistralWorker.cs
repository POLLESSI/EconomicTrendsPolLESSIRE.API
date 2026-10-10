using EconomicTrendsPolLESSIRE.Application.Mistral;
using EconomicTrendsPolLESSIRE.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Worker.Mistral
{
    public sealed class MistralWorker : BackgroundService
    {
        private readonly IMistralBackgroundQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MistralWorker> _logger;

        public MistralWorker(IMistralBackgroundQueue queue, IServiceScopeFactory scopeFactory, ILogger<MistralWorker> logger)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("[Mistral-WORKER] ExecuteAsync entered.");

            _logger.LogInformation("[Mistral-WORKER] Started.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var workItem = await _queue.DequeueAsync(stoppingToken);
                    await ProcessAsync(workItem, stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Normal application shutdown.
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "[Mistral-WORKER] Fatal worker failure.");
                throw;
            }
            finally
            {
                Console.WriteLine("[Mistral-WORKER] ExecuteAsync stopped.");
                _logger.LogInformation("[Mistral-WORKER] Stopped.");
            }
        }

        private async Task ProcessAsync(MistralWorkItem workItem, CancellationToken stoppingToken)
        {
            Console.WriteLine(
                $"[Mistral-WORKER] Processing started. " +
                $"InteractionId={workItem.Interaction.Id}, " +
                $"RequestId={workItem.RequestId}");

            _logger.LogInformation(
                "[Mistral-WORKER] Processing started. " +
                "InteractionId={InteractionId}, " +
                "RequestId={RequestId}",
                workItem.Interaction.Id,
                workItem.RequestId);

            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();

                var processor = scope.ServiceProvider.GetRequiredService<IMistralQueuedRequestProcessor>();

                await processor.ProcessQueuedAsync(workItem, stoppingToken);

                Console.WriteLine(
                    $"[Mistral-WORKER] Processing finished. " +
                    $"InteractionId={workItem.Interaction.Id}, " +
                    $"RequestId={workItem.RequestId}");

                _logger.LogInformation(
                    "[Mistral-WORKER] Processing finished. " +
                    "InteractionId={InteractionId}, " +
                    "RequestId={RequestId}",
                    workItem.Interaction.Id,
                    workItem.RequestId);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "[Mistral-WORKER] Processing interrupted because application is stopping. " +
                    "InteractionId={InteractionId}",
                    workItem.Interaction.Id);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "[Mistral-WORKER] Unexpected processing failure. " +
                    "InteractionId={InteractionId}, " +
                    "RequestId={RequestId}",
                    workItem.Interaction.Id,
                    workItem.RequestId);
            }
        }
    }
}










































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.