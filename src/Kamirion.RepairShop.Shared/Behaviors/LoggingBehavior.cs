using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kamirion.RepairShop.Shared.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var sw = Stopwatch.StartNew();
        try
        {
            var response = await next();
            logger.LogInformation("Handled {Request} in {Elapsed}ms", requestName, sw.ElapsedMilliseconds);
            return response;
        }
        catch
        {
            logger.LogWarning("Failed {Request} after {Elapsed}ms", requestName, sw.ElapsedMilliseconds);
            throw;
        }
    }
}
