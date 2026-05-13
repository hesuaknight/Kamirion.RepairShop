using Kamirion.RepairShop.Tenancy.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Kamirion.RepairShop.Infrastructure.Tenancy;

internal sealed class TenantLookupService(AppDbContext db) : ITenantLookupService
{
    public Task<string?> FindIdBySlugAsync(string slug, CancellationToken ct = default) =>
        db.Tenants
          .Where(t => t.Slug == slug && t.IsActive)
          .Select(t => (string?)t.Id)
          .FirstOrDefaultAsync(ct);
}
