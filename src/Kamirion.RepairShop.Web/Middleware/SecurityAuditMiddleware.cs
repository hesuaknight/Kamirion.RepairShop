using System.Security.Claims;
using Kamirion.RepairShop.Identity.Contracts;
using Kamirion.RepairShop.Shared.Identity;

namespace Kamirion.RepairShop.Web.Middleware;

public sealed class SecurityAuditMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ISecurityAuditService securityAuditService,
        ITenantContext tenantContext)
    {
        await next(context);

        if (context.User.Identity?.IsAuthenticated != true)
            return;

        if (IsExcludedPath(context.Request.Path))
            return;

        var eventType = context.Response.StatusCode == StatusCodes.Status403Forbidden
            ? SecurityEventTypes.AccessDenied
            : SecurityEventTypes.AuthenticatedRequest;

        var tenantId = tenantContext.IsResolved ? tenantContext.TenantId : null;
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = context.User.FindFirstValue(ClaimTypes.Email);

        await securityAuditService.RecordAsync(
            eventType: eventType,
            success: context.Response.StatusCode < 400,
            tenantId: tenantId,
            userId: userId,
            userEmail: userEmail,
            resourcePath: context.Request.Path,
            cancellationToken: CancellationToken.None);
    }

    private static bool IsExcludedPath(PathString path) =>
        path.StartsWithSegments("/health") ||
        path.StartsWithSegments("/favicon") ||
        path.StartsWithSegments("/css") ||
        path.StartsWithSegments("/js") ||
        path.StartsWithSegments("/lib");
}
