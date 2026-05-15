using Kamirion.RepairShop.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kamirion.RepairShop.Infrastructure.Configurations;

internal sealed class SecurityAuditLogConfiguration : IEntityTypeConfiguration<SecurityAuditLog>
{
    public void Configure(EntityTypeBuilder<SecurityAuditLog> builder)
    {
        builder.ToTable("SecurityAuditLogs");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(26).IsRequired();
        builder.Property(e => e.TenantId).HasMaxLength(26);
        builder.Property(e => e.UserId).HasMaxLength(450);
        builder.Property(e => e.UserEmail).HasMaxLength(256);
        builder.Property(e => e.EventType).HasMaxLength(100).IsRequired();
        builder.Property(e => e.IpAddress).HasMaxLength(45);
        builder.Property(e => e.UserAgent).HasMaxLength(500);
        builder.Property(e => e.ResourcePath).HasMaxLength(500);
        builder.Property(e => e.AdditionalData).HasColumnType("nvarchar(max)");
        builder.Property(e => e.Success).IsRequired();
        builder.Property(e => e.OccurredAt).IsRequired();

        builder.HasIndex(e => e.TenantId);
        builder.HasIndex(e => e.OccurredAt);
        builder.HasIndex(e => e.EventType);
    }
}
