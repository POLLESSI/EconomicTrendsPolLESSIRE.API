using EconomicTrendsPolLESSIRE.Shared.Observability;
using EconomicTrendsPolLESSIRE.Shared.Resilience;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;

namespace EconomicTrendsPolLESSIRE.Application.Behaviors
{
    public class ResilienceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly ResiliencePipeline<TResponse> _pipeline;
        private readonly ILogger<ResilienceBehavior<TRequest, TResponse>> _logger;

        public ResilienceBehavior(ILogger<ResilienceBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
            _pipeline = ResilienceExec.BuildPipelineFor<TResponse>(_logger);
        }

        public Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            var (ctx, _) = ObservabilityContext.Create(
                service: "application",
                operation: typeof(TRequest).Name);

            return ResilienceExec.ExecuteLoggedAsync(
                _pipeline,
                _ => new ValueTask<TResponse>(next()),
                ctx,
                _logger,
                policyName: "cqrs",
                cancellationToken: ct);
        }
    }
}

































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.