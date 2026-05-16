using Kamirion.RepairShop.Notifications.Contracts;
using Kamirion.RepairShop.Notifications.Domain;
using Kamirion.RepairShop.Shared.Realtime;
using Kamirion.RepairShop.Shared.Results;

namespace Kamirion.RepairShop.Notifications.Application;

public sealed class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly IRealtimeNotifier _realtimeNotifier;

    public NotificationService(INotificationRepository repository, IRealtimeNotifier realtimeNotifier)
    {
        _repository = repository;
        _realtimeNotifier = realtimeNotifier;
    }

    public async Task<Result> SendAsync(
        string tenantId,
        string userId,
        string type,
        string title,
        string body,
        string? actionUrl = null,
        CancellationToken cancellationToken = default)
    {
        var notification = Notification.Create(tenantId, userId, type, title, body, actionUrl);
        await _repository.AddAsync(notification, cancellationToken);

        await _realtimeNotifier.NotifyUserAsync(
            userId,
            "notification:new",
            new { notification.Id, notification.Type, notification.Title, notification.Body, notification.ActionUrl },
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> MarkAsReadAsync(
        string notificationId,
        string callerUserId,
        CancellationToken cancellationToken = default)
    {
        var notification = await _repository.GetByIdAsync(notificationId, cancellationToken);
        if (notification is null)
            return Result.Failure(Error.NotFound("Notification"));

        if (notification.UserId != callerUserId)
            return Result.Failure(Error.Unauthorized());

        var result = notification.MarkAsRead();
        if (result.IsFailure)
            return result;

        await _repository.UpdateAsync(notification, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<NotificationResponse>>> GetUnreadAsync(
        string tenantId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var notifications = await _repository.GetUnreadAsync(tenantId, userId, cancellationToken);

        var responses = notifications
            .Select(n => new NotificationResponse(
                n.Id, n.Type, n.Title, n.Body, n.ActionUrl, n.IsRead, n.CreatedAt, n.ReadAt))
            .ToList();

        return Result.Success<IReadOnlyList<NotificationResponse>>(responses);
    }
}
