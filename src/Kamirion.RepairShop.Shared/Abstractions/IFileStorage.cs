namespace Kamirion.RepairShop.Shared.Abstractions;

public interface IFileStorage
{
    Task<string> UploadAsync(string key, Stream content, string contentType, CancellationToken ct = default);

    Task<string> GetPresignedUrlAsync(string key, TimeSpan expiry, CancellationToken ct = default);

    Task DeleteAsync(string key, CancellationToken ct = default);
}
