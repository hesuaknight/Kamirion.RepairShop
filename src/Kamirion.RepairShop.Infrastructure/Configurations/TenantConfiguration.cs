using Kamirion.RepairShop.Tenancy.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kamirion.RepairShop.Infrastructure.Configurations;

internal sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasMaxLength(26).IsRequired();
        builder.Property(t => t.Slug).HasMaxLength(100).IsRequired();
        builder.HasIndex(t => t.Slug).IsUnique();
        builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
        builder.Property(t => t.IsActive).IsRequired();
    }
}
