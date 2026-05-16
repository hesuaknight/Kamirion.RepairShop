using Kamirion.RepairShop.Shared.Abstractions;
using Kamirion.RepairShop.Shared.Domain;
using Kamirion.RepairShop.Shared.Results;
using Kamirion.RepairShop.Shared.Utils;

namespace Kamirion.RepairShop.Notifications.Domain;

public sealed class Notification : Entity<string>, ITenantOwned
{
    public string TenantId { get; private set; } = string.Empty;
    public string UserId { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Body { get; private set; } = string.Empty;
    public string? ActionUrl { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ReadAt { get; private set; }

    private Notification() { }

    public static Notification Create(
        string tenantId,
        string userId,
        string type,
        string title,
        string body,
        string? actionUrl = null)
    {
        return new Notification
        {
            Id = UlidGenerator.New(),
            TenantId = tenantId,
            UserId = userId,
            Type = type,
            Title = title,
            Body = body,
            ActionUrl = actionUrl,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public Result MarkAsRead()
    {
        if (IsRead)
            return Result.Failure(Error.Conflict("Notification_AlreadyRead_Error"));

        IsRead = true;
        ReadAt = DateTime.UtcNow;
        return Result.Success();
    }
}
