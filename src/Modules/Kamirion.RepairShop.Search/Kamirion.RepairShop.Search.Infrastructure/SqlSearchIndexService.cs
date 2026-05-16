using Kamirion.RepairShop.Search.Contracts;
using Kamirion.RepairShop.Search.Domain;

namespace Kamirion.RepairShop.Search.Infrastructure;

internal sealed class SqlSearchIndexService : ISearchIndexService
{
    private readonly ISearchIndexRepository _repository;

    public SqlSearchIndexService(ISearchIndexRepository repository)
    {
        _repository = repository;
    }

    public async Task IndexAsync(
        string tenantId,
        string entityType,
        string entityId,
        string searchableText,
        string displayTitle,
        string? displaySubtitle,
        string url,
        CancellationToken cancellationToken = default)
    {
        var entry = SearchIndexEntry.Create(
            tenantId, entityType, entityId,
            searchableText, displayTitle, displaySubtitle, url);

        await _repository.UpsertAsync(entry, cancellationToken);
    }

    public async Task RemoveAsync(
        string tenantId,
        string entityType,
        string entityId,
        CancellationToken cancellationToken = default)
    {
        await _repository.RemoveAsync(tenantId, entityType, entityId, cancellationToken);
    }
}
