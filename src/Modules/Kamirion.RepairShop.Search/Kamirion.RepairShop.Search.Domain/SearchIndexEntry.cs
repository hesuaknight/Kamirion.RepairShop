using Kamirion.RepairShop.Shared.Abstractions;
using Kamirion.RepairShop.Shared.Domain;
using Kamirion.RepairShop.Shared.Utils;

namespace Kamirion.RepairShop.Search.Domain;

public class SearchIndexEntry : Entity<string>, ITenantOwned
{
    public string TenantId { get; private set; } = string.Empty;
    public string EntityType { get; private set; } = string.Empty;
    public string EntityId { get; private set; } = string.Empty;
    public string SearchableText { get; private set; } = string.Empty;
    public string DisplayTitle { get; private set; } = string.Empty;
    public string? DisplaySubtitle { get; private set; }
    public string Url { get; private set; } = string.Empty;
    public DateTime IndexedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    private SearchIndexEntry() { }

    public static SearchIndexEntry Create(
        string tenantId,
        string entityType,
        string entityId,
        string searchableText,
        string displayTitle,
        string? displaySubtitle,
        string url)
    {
        return new SearchIndexEntry
        {
            Id = UlidGenerator.New(),
            TenantId = tenantId,
            EntityType = entityType,
            EntityId = entityId,
            SearchableText = searchableText,
            DisplayTitle = displayTitle,
            DisplaySubtitle = displaySubtitle,
            Url = url,
            IndexedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public void Update(string searchableText, string displayTitle, string? displaySubtitle, string url)
    {
        SearchableText = searchableText;
        DisplayTitle = displayTitle;
        DisplaySubtitle = displaySubtitle;
        Url = url;
        IndexedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void MarkDeleted()
    {
        IsDeleted = true;
    }
}
