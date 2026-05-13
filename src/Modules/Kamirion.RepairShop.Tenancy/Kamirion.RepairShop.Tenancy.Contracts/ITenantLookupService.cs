namespace Kamirion.RepairShop.Tenancy.Contracts;

public interface ITenantLookupService
{
    Task<string?> FindIdBySlugAsync(string slug, CancellationToken ct = default);
}
