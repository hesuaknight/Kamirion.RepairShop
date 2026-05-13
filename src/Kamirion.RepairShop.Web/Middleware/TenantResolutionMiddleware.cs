using Kamirion.RepairShop.Infrastructure.Tenancy;
using Kamirion.RepairShop.Tenancy.Contracts;

namespace Kamirion.RepairShop.Web.Middleware;

public sealed class TenantResolutionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        TenantContext tenantContext,
        ITenantLookupService tenantLookupService)
    {
        var host = context.Request.Host.Host;
        var parts = host.Split('.');

        // Sin subdominio (ej: localhost) → continuar sin resolver
        if (parts.Length < 3)
        {
            await next(context);
            return;
        }

        var slug = parts[0];
        var tenantId = await tenantLookupService.FindIdBySlugAsync(slug, context.RequestAborted);

        if (tenantId is null)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        tenantContext.Resolve(tenantId);
        await next(context);
    }
}
