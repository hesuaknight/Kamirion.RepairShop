using Kamirion.RepairShop.Search.Domain;

namespace Kamirion.RepairShop.Search.Contracts;

public interface ISearchIndexRepository
{
    Task<IReadOnlyList<SearchIndexEntry>> SearchAsync(
        string tenantId,
        string query,
        SearchEntityType[]? entityTypes = null,
        int maxResults = 20,
        CancellationToken cancellationToken = default);

    Task UpsertAsync(SearchIndexEntry entry, CancellationToken cancellationToken = default);

    Task RemoveAsync(
        string tenantId,
        string entityType,
        string entityId,
        CancellationToken cancellationToken = default);
}
