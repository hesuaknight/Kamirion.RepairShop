using Kamirion.RepairShop.Shared.Identity;

namespace Kamirion.RepairShop.Infrastructure.Tenancy;

public sealed class TenantContext : ITenantContext
{
    public string TenantId { get; private set; } = string.Empty;
    public bool IsResolved { get; private set; }

    public void Resolve(string tenantId)
    {
        TenantId = tenantId;
        IsResolved = true;
    }
}
