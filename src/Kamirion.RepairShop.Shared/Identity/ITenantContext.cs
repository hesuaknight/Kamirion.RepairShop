namespace Kamirion.RepairShop.Shared.Identity;

public interface ITenantContext
{
    string TenantId { get; }
    bool IsResolved { get; }
}
