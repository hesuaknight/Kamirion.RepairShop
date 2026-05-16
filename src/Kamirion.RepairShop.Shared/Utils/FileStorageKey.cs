namespace Kamirion.RepairShop.Shared.Utils;

public static class FileStorageKey
{
    /// <summary>
    /// Convención: tenants/{tenantId}/{entity}/{entityId}/{type}/{fileId}.{ext}
    /// </summary>
    public static string Build(
        string tenantId,
        string entity,
        string entityId,
        string type,
        string fileId,
        string extension)
    {
        var ext = extension.TrimStart('.').ToLowerInvariant();
        return $"tenants/{tenantId}/{entity}/{entityId}/{type}/{fileId}.{ext}";
    }
}
