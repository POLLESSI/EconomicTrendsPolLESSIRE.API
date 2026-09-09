using MediatR;

namespace EconomicTrendsPolLESSIRE.Application.Behaviors
{
    public interface IResilienceBehavior<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct);
    }
}
