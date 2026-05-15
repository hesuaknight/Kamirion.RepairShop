using Kamirion.RepairShop.Shared.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kamirion.RepairShop.Shared.Behaviors;

public sealed class ExceptionBehavior<TRequest, TResponse>(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception processing {Request}", typeof(TRequest).Name);
            var error = new Error("UnhandledException", "An unexpected error occurred.");
            return ResultFactory.Failure<TResponse>(error);
        }
    }
}
