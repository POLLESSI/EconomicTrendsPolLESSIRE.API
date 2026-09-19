using MediatR;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Application.Common.MediaR
{
    public abstract class HandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        protected HandlerBase(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("📨 Handling {RequestType}", typeof(TRequest).Name);
                var response = await HandleRequest(request, cancellationToken);
                _logger.LogInformation("✅ Handled {RequestType} successfully", typeof(TRequest).Name);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error handling {RequestType}", typeof(TRequest).Name);
                throw;
            }
        }
        protected abstract Task<TResponse> HandleRequest(TRequest request, CancellationToken cancellationToken);
    }
}






























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.