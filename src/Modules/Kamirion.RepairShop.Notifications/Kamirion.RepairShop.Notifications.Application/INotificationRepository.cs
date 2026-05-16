using Kamirion.RepairShop.Notifications.Domain;

namespace Kamirion.RepairShop.Notifications.Application;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<Notification?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Notification>> GetUnreadAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
}
