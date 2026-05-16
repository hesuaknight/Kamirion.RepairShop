using Kamirion.RepairShop.Shared.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Kamirion.RepairShop.Infrastructure.Storage;

internal sealed class LocalFileStorage : IFileStorage
{
    private const string UploadsFolder = "uploads";
    private readonly string _uploadsRootPath;
    private readonly ILogger<LocalFileStorage> _logger;

    public LocalFileStorage(IWebHostEnvironment environment, ILogger<LocalFileStorage> logger)
    {
        _uploadsRootPath = Path.Combine(environment.WebRootPath, UploadsFolder);
        _logger = logger;
    }

    public async Task<string> UploadAsync(string key, Stream content, string contentType, CancellationToken ct = default)
    {
        var filePath = GetFullPath(key);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        await using var fileStream = File.Create(filePath);
        await content.CopyToAsync(fileStream, ct);

        _logger.LogInformation("Archivo guardado localmente. Key: {Key}", key);
        return key;
    }

    public Task<string> GetPresignedUrlAsync(string key, TimeSpan expiry, CancellationToken ct = default)
    {
        var url = $"/{UploadsFolder}/{key.Replace('\\', '/')}";
        return Task.FromResult(url);
    }

    public Task DeleteAsync(string key, CancellationToken ct = default)
    {
        var filePath = GetFullPath(key);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            _logger.LogInformation("Archivo local eliminado. Key: {Key}", key);
        }

        return Task.CompletedTask;
    }

    private string GetFullPath(string key) =>
        Path.Combine(_uploadsRootPath, key.Replace('/', Path.DirectorySeparatorChar));
}
