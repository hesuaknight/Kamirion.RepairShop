namespace Kamirion.RepairShop.Shared.Domain;

public interface IDomainEventSource
{
    IReadOnlyList<IDomainEvent> GetPendingDomainEvents();
    void ClearAllDomainEvents();
}
