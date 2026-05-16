using Kamirion.RepairShop.Search.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kamirion.RepairShop.Infrastructure.Configurations;

internal sealed class SearchIndexEntryConfiguration : IEntityTypeConfiguration<SearchIndexEntry>
{
    public void Configure(EntityTypeBuilder<SearchIndexEntry> builder)
    {
        builder.ToTable("SearchIndexEntries");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(26).IsRequired();

        builder.Property(e => e.TenantId).HasMaxLength(26).IsRequired();
        builder.Property(e => e.EntityType).HasMaxLength(50).IsRequired();
        builder.Property(e => e.EntityId).HasMaxLength(26).IsRequired();
        builder.Property(e => e.SearchableText).IsRequired();
        builder.Property(e => e.DisplayTitle).HasMaxLength(500).IsRequired();
        builder.Property(e => e.DisplaySubtitle).HasMaxLength(500);
        builder.Property(e => e.Url).HasMaxLength(500).IsRequired();
        builder.Property(e => e.IndexedAt).IsRequired();
        builder.Property(e => e.IsDeleted).IsRequired();

        builder.HasIndex(e => new { e.TenantId, e.EntityType, e.EntityId })
            .IsUnique()
            .HasDatabaseName("IX_SearchIndexEntries_TenantId_EntityType_EntityId");

        builder.HasIndex(e => e.IsDeleted)
            .HasDatabaseName("IX_SearchIndexEntries_IsDeleted");
    }
}
