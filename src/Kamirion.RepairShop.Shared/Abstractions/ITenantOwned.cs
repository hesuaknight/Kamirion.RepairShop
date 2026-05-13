namespace Kamirion.RepairShop.Shared.Abstractions;

public interface ITenantOwned
{
    string TenantId { get; }
}
