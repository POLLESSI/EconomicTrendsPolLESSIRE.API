using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace EconomicTrendsPolLESSIRE.Shared.Resilience
{
    public static class ResilienceExec
    {
        // Native .NET metrics
        private static readonly Meter ResilienceMeter = new("EconomicTrendsPolLESSIRE.Resilience", "1.0.0");
        public static readonly Counter<long> FailureCounter = ResilienceMeter.CreateCounter<long>("economictrends_resilience_failures_total", unit: "{failure}",  description: "Total number of resilience pipeline failures.");
        public static readonly Histogram<double> DurationHistogram = ResilienceMeter.CreateHistogram<double>("economictrends_resilience_duration_ms", unit: "ms", description: "Execution duration of resilience pipelines.");

        // Keys
        private static readonly ResiliencePropertyKey<string> ServiceKey = new("service");
        private static readonly ResiliencePropertyKey<string> OperationKey = new("operation");
        private static readonly ResiliencePropertyKey<string> CorrelationIdKey = new("correlationId");

        public static async Task<T> ExecuteLoggedAsync<T>(ResiliencePipeline<T> pipeline, Func<CancellationToken, ValueTask<T>> action, ResilienceContext ctx, ILogger logger, string policyName, CancellationToken cancellationToken = default)
        {
            var service = ctx.Properties.GetValue(ServiceKey, "");
            var operation = ctx.Properties.GetValue(OperationKey, "");
            var corrId = ctx.Properties.GetValue(CorrelationIdKey, "");

            using var act = new Activity($"resilience:{service}/{operation}");

            act.AddTag("policy", policyName);
            act.AddTag("service", service);
            act.AddTag("operation", operation);
            act.AddTag("correlationId", corrId);

            act.Start();

            var sw = Stopwatch.StartNew();

            try
            {
                var result = await pipeline.ExecuteAsync(_ => action(cancellationToken), ctx);

                sw.Stop();

                var tags = new TagList
                {
                    { "policy", policyName },
                    { "service", service },
                    { "operation", operation }
                };

                DurationHistogram.Record(sw.Elapsed.TotalMilliseconds, tags);

                return result;
            }
            catch (Exception ex)
            {
                sw.Stop();

                var durationTags = new TagList
                {
                    { "policy", policyName },
                    { "service", service },
                    { "operation", operation }
                };

                DurationHistogram.Record(sw.Elapsed.TotalMilliseconds, durationTags);

                var failureTags = new TagList
                {
                    { "policy", policyName },
                    { "service", service },
                    { "operation", operation },
                    { "exception", ex.GetType().Name }
                };

                FailureCounter.Add(1, failureTags);

                logger.LogError(ex, "Resilience failure {Policy} {Service}/{Operation} corr={CorrelationId}", policyName, service, operation, corrId);

                throw;
            }
            finally
            {
                act.Stop();

                ResilienceContextPool.Shared.Return(ctx);
            }
        }

        public static ResiliencePipeline<T> BuildPipelineFor<T>(ILogger logger) =>
            new ResiliencePipelineBuilder<T>()
                .AddTimeout(
                    new TimeoutStrategyOptions
                    {
                        Timeout = TimeSpan.FromSeconds(15)
                    })
                .AddRetry(
                    new RetryStrategyOptions<T>
                    {
                        MaxRetryAttempts = 3
                    })
                .AddCircuitBreaker(
                    new CircuitBreakerStrategyOptions<T>
                    {
                        FailureRatio = 0.5,
                        MinimumThroughput = 5,
                        BreakDuration = TimeSpan.FromSeconds(30),

                        OnOpened = _ =>
                        {
                            logger.LogWarning("CQRS circuit opened");

                            return default;
                        },

                        OnClosed = _ =>
                        {
                            logger.LogInformation("CQRS circuit closed");

                            return default;
                        }
                    })
                .Build();
    }
}