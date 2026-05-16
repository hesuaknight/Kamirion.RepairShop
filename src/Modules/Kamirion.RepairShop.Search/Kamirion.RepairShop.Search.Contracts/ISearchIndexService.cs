namespace Kamirion.RepairShop.Search.Contracts;

public interface ISearchIndexService
{
    Task IndexAsync(
        string tenantId,
        string entityType,
        string entityId,
        string searchableText,
        string displayTitle,
        string? displaySubtitle,
        string url,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(
        string tenantId,
        string entityType,
        string entityId,
        CancellationToken cancellationToken = default);
}
