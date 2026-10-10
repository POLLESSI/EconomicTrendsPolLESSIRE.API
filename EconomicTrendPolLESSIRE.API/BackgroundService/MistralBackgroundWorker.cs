using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Application.Mistral;

namespace EconomicTrendsPolLESSIRE.API.BackgroundServices
{
    public sealed class MistralBackgroundWorker
        : BackgroundService
    {
        private readonly IMistralBackgroundQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MistralBackgroundWorker> _logger;

        public MistralBackgroundWorker(
            IMistralBackgroundQueue queue,
            IServiceScopeFactory scopeFactory,
            ILogger<MistralBackgroundWorker> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Mistral background worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                MistralWorkItem workItem;

                try
                {
                    workItem =
                        await _queue.DequeueAsync(
                            stoppingToken);
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    await using var scope =
                        _scopeFactory.CreateAsyncScope();

                    var processor =
                        scope.ServiceProvider
                            .GetRequiredService<
                                IMistralQueuedRequestProcessor>();

                    await processor.ProcessQueuedAsync(
                        workItem,
                        stoppingToken);
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error while processing Mistral background request.");
                }
            }

            _logger.LogInformation(
                "Mistral background worker stopped.");
        }
    }
}




















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.