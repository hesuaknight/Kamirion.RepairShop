using Kamirion.RepairShop.Communication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kamirion.RepairShop.Infrastructure.Configurations;

internal sealed class MessageTemplateConfiguration : IEntityTypeConfiguration<MessageTemplate>
{
    public void Configure(EntityTypeBuilder<MessageTemplate> builder)
    {
        builder.ToTable("MessageTemplates");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasMaxLength(26).IsRequired();

        builder.Property(t => t.TenantId).HasMaxLength(26).IsRequired();
        builder.Property(t => t.TemplateKey).HasMaxLength(100).IsRequired();
        builder.Property(t => t.Channel).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Culture).HasMaxLength(10).IsRequired();
        builder.Property(t => t.TwilioContentSid).HasMaxLength(100);
        builder.Property(t => t.BodyTemplate).IsRequired();
        builder.Property(t => t.IsActive).IsRequired();

        builder.HasIndex(t => new { t.TenantId, t.TemplateKey, t.Channel, t.Culture })
            .IsUnique()
            .HasDatabaseName("IX_MessageTemplates_TenantId_TemplateKey_Channel_Culture");
    }
}
