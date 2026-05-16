namespace Kamirion.RepairShop.Notifications.Contracts;

public record NotificationResponse(
    string Id,
    string Type,
    string Title,
    string Body,
    string? ActionUrl,
    bool IsRead,
    DateTime CreatedAt,
    DateTime? ReadAt);
