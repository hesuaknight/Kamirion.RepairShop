using Kamirion.RepairShop.Shared.Results;

namespace Kamirion.RepairShop.Notifications.Contracts;

public interface INotificationService
{
    Task<Result> SendAsync(
        string tenantId,
        string userId,
        string type,
        string title,
        string body,
        string? actionUrl = null,
        CancellationToken cancellationToken = default);

    Task<Result> MarkAsReadAsync(
        string notificationId,
        string callerUserId,
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<NotificationResponse>>> GetUnreadAsync(
        string tenantId,
        string userId,
        CancellationToken cancellationToken = default);
}
