using Kamirion.RepairShop.Shared.Domain;
using MediatR;

namespace Kamirion.RepairShop.Shared.Behaviors;

public sealed class DomainEventDispatchBehavior<TRequest, TResponse>(
    IDomainEventSource eventSource,
    IDomainEventDispatcher dispatcher)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        var events = eventSource.GetPendingDomainEvents();
        eventSource.ClearAllDomainEvents();
        foreach (var domainEvent in events)
            await dispatcher.DispatchAsync(domainEvent, cancellationToken);

        return response;
    }
}
