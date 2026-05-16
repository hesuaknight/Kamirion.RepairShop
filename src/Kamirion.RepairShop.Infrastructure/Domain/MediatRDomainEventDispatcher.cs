using Kamirion.RepairShop.Shared.Domain;
using MediatR;

namespace Kamirion.RepairShop.Infrastructure.Domain;

internal sealed class MediatRDomainEventDispatcher(IPublisher publisher) : IDomainEventDispatcher
{
    public Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default) =>
        publisher.Publish((INotification)domainEvent, cancellationToken);
}
