using Kamirion.RepairShop.Notifications.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kamirion.RepairShop.Infrastructure.Configurations;

internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).HasMaxLength(26).IsRequired();

        builder.Property(n => n.TenantId).HasMaxLength(26).IsRequired();
        builder.Property(n => n.UserId).HasMaxLength(450).IsRequired();
        builder.Property(n => n.Type).HasMaxLength(50).IsRequired();
        builder.Property(n => n.Title).HasMaxLength(200).IsRequired();
        builder.Property(n => n.Body).HasMaxLength(1000).IsRequired();
        builder.Property(n => n.ActionUrl).HasMaxLength(500);
        builder.Property(n => n.IsRead).IsRequired();
        builder.Property(n => n.CreatedAt).IsRequired();
        builder.Property(n => n.ReadAt);

        builder.HasIndex(n => new { n.TenantId, n.UserId, n.IsRead, n.CreatedAt })
            .HasDatabaseName("IX_Notifications_TenantId_UserId_IsRead_CreatedAt");
    }
}
