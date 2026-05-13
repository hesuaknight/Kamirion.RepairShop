using Kamirion.RepairShop.Shared.Domain;
using Kamirion.RepairShop.Shared.Results;
using Kamirion.RepairShop.Shared.Utils;

namespace Kamirion.RepairShop.Tenancy.Domain;

public sealed class Tenant : Entity<string>
{
    public string Slug { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private Tenant() { }

    public static Result<Tenant> Create(string slug, string name)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<Tenant>(Error.Validation(nameof(Slug), "Tenant_Slug_Required"));

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Tenant>(Error.Validation(nameof(Name), "Tenant_Name_Required"));

        return Result.Success(new Tenant
        {
            Id = UlidGenerator.New(),
            Slug = slug.ToLowerInvariant().Trim(),
            Name = name.Trim(),
            IsActive = true
        });
    }

    public void Deactivate() => IsActive = false;
}
