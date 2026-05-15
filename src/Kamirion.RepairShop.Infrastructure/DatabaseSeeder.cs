using Kamirion.RepairShop.Communication.Domain;
using Kamirion.RepairShop.Tenancy.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kamirion.RepairShop.Infrastructure;

public static class DatabaseSeeder
{
    private const string DevTenantSlug = "dev";

    public static async Task SeedDevelopmentDataAsync(AppDbContext context)
    {
        var tenantId = await EnsureDevelopmentTenantAsync(context);
        await EnsureBaseTemplatesAsync(context, tenantId);
        await context.SaveChangesAsync();
    }

    private static async Task<string> EnsureDevelopmentTenantAsync(AppDbContext context)
    {
        var tenant = await context.Tenants
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.Slug == DevTenantSlug);

        if (tenant is not null)
            return tenant.Id;

        var result = Tenant.Create(DevTenantSlug, "Development Tenant");
        tenant = result.Value;
        context.Tenants.Add(tenant);
        return tenant.Id;
    }

    private static async Task EnsureBaseTemplatesAsync(AppDbContext context, string tenantId)
    {
        var existing = await context.MessageTemplates
            .IgnoreQueryFilters()
            .Where(t => t.TenantId == tenantId && t.Channel == MessageChannel.WhatsApp && t.Culture == "es")
            .Select(t => t.TemplateKey)
            .ToListAsync();

        var templates = new[]
        {
            (
                Key: "RepairTicket_StatusChanged",
                Body: "Tu reparación {{ticketNumber}} cambió de estado a {{status}}. Para más información comunicate con nosotros."
            ),
            (
                Key: "RepairTicket_ReadyForPickup",
                Body: "¡Tu dispositivo está listo! La reparación {{ticketNumber}} fue completada. Podés pasar a retirarlo en nuestro local."
            ),
            (
                Key: "Estimate_ApprovalRequest",
                Body: "Te enviamos el presupuesto de la reparación {{ticketNumber}} por un total de {{amount}}. Respondé ACEPTAR para confirmar o RECHAZAR para cancelar."
            )
        };

        foreach (var (key, body) in templates)
        {
            if (existing.Contains(key))
                continue;

            var template = MessageTemplate.Create(
                tenantId: tenantId,
                templateKey: key,
                channel: MessageChannel.WhatsApp,
                culture: "es",
                bodyTemplate: body).Value;

            context.MessageTemplates.Add(template);
        }
    }
}
