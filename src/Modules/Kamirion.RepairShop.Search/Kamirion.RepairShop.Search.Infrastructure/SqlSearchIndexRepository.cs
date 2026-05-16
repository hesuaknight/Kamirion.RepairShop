using System.Text.RegularExpressions;
using Kamirion.RepairShop.Infrastructure;
using Kamirion.RepairShop.Search.Contracts;
using Kamirion.RepairShop.Search.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kamirion.RepairShop.Search.Infrastructure;

internal sealed class SqlSearchIndexRepository : ISearchIndexRepository
{
    private readonly AppDbContext _context;

    public SqlSearchIndexRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SearchIndexEntry>> SearchAsync(
        string tenantId,
        string query,
        SearchEntityType[]? entityTypes = null,
        int maxResults = 20,
        CancellationToken cancellationToken = default)
    {
        var sanitized = SanitizeFtsQuery(query);

        IQueryable<SearchIndexEntry> baseQuery = _context.SearchIndexEntries
            .IgnoreQueryFilters()
            .Where(e => e.TenantId == tenantId && !e.IsDeleted);

        if (entityTypes is { Length: > 0 })
        {
            var typeNames = entityTypes.Select(t => t.ToString()).ToArray();
            baseQuery = baseQuery.Where(e => typeNames.Contains(e.EntityType));
        }

        if (!string.IsNullOrWhiteSpace(sanitized))
        {
            baseQuery = baseQuery.Where(e => EF.Functions.Contains(e.SearchableText, sanitized));
        }
        else
        {
            // Fallback a LIKE cuando la query queda vacía tras sanitización
            var likePattern = $"%{query.Trim()}%";
            baseQuery = baseQuery.Where(e => EF.Functions.Like(e.SearchableText, likePattern));
        }

        return await baseQuery
            .OrderByDescending(e => e.IndexedAt)
            .Take(maxResults)
            .ToListAsync(cancellationToken);
    }

    public async Task UpsertAsync(SearchIndexEntry entry, CancellationToken cancellationToken = default)
    {
        var existing = await _context.SearchIndexEntries
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                e => e.TenantId == entry.TenantId
                  && e.EntityType == entry.EntityType
                  && e.EntityId == entry.EntityId,
                cancellationToken);

        if (existing is null)
        {
            _context.SearchIndexEntries.Add(entry);
        }
        else
        {
            existing.Update(entry.SearchableText, entry.DisplayTitle, entry.DisplaySubtitle, entry.Url);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(
        string tenantId,
        string entityType,
        string entityId,
        CancellationToken cancellationToken = default)
    {
        var entry = await _context.SearchIndexEntries
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                e => e.TenantId == tenantId
                  && e.EntityType == entityType
                  && e.EntityId == entityId,
                cancellationToken);

        if (entry is not null)
        {
            entry.MarkDeleted();
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    // Escapa caracteres especiales de FTS y construye una expresión de prefix search
    private static string SanitizeFtsQuery(string query)
    {
        // Eliminar caracteres con significado especial en SQL Server FTS
        var cleaned = Regex.Replace(query, @"[""\(\)\{\}\[\]\|&~\^!\-\*%]", " ");
        var words = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (words.Length == 0)
            return string.Empty;

        // Envolver cada palabra como prefix search: "palabra*"
        return string.Join(" AND ", words.Select(w => $"\"{w}*\""));
    }
}
