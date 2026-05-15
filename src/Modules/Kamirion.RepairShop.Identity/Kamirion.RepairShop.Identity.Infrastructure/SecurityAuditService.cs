using System.Text.Json;
using Kamirion.RepairShop.Identity.Contracts;
using Kamirion.RepairShop.Infrastructure;
using Kamirion.RepairShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Kamirion.RepairShop.Identity.Infrastructure;

internal sealed class SecurityAuditService(
    AppDbContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    ILogger<SecurityAuditService> logger) : ISecurityAuditService
{
    public async Task RecordAsync(
        string eventType,
        bool success,
        string? tenantId = null,
        string? userId = null,
        string? userEmail = null,
        string? resourcePath = null,
        object? additionalData = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var context = httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection.RemoteIpAddress?.ToString();
            var userAgent = context?.Request.Headers.UserAgent.ToString();

            string? additionalDataJson = additionalData is not null
                ? JsonSerializer.Serialize(additionalData)
                : null;

            var entry = SecurityAuditLog.Create(
                eventType: eventType,
                success: success,
                tenantId: tenantId,
                userId: userId,
                userEmail: userEmail,
                ipAddress: ipAddress,
                userAgent: userAgent,
                resourcePath: resourcePath,
                additionalData: additionalDataJson);

            await dbContext.SecurityAuditLogs.AddAsync(entry, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to record security audit event {EventType}", eventType);
        }
    }
}
