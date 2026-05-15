using Kamirion.RepairShop.Shared.Utils;

namespace Kamirion.RepairShop.Infrastructure.Identity;

public sealed class SecurityAuditLog
{
    public string Id { get; private set; } = default!;
    public string? TenantId { get; private set; }
    public string? UserId { get; private set; }
    public string? UserEmail { get; private set; }
    public string EventType { get; private set; } = default!;
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public string? ResourcePath { get; private set; }
    public string? AdditionalData { get; private set; }
    public bool Success { get; private set; }
    public DateTime OccurredAt { get; private set; }

    private SecurityAuditLog() { }

    public static SecurityAuditLog Create(
        string eventType,
        bool success,
        string? tenantId = null,
        string? userId = null,
        string? userEmail = null,
        string? ipAddress = null,
        string? userAgent = null,
        string? resourcePath = null,
        string? additionalData = null)
    {
        return new SecurityAuditLog
        {
            Id = UlidGenerator.New(),
            EventType = eventType,
            Success = success,
            TenantId = tenantId,
            UserId = userId,
            UserEmail = userEmail,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            ResourcePath = resourcePath,
            AdditionalData = additionalData,
            OccurredAt = DateTime.UtcNow
        };
    }
}
