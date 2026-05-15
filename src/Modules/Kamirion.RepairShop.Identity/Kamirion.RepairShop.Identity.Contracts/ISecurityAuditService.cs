namespace Kamirion.RepairShop.Identity.Contracts;

public interface ISecurityAuditService
{
    Task RecordAsync(
        string eventType,
        bool success,
        string? tenantId = null,
        string? userId = null,
        string? userEmail = null,
        string? resourcePath = null,
        object? additionalData = null,
        CancellationToken cancellationToken = default);
}
