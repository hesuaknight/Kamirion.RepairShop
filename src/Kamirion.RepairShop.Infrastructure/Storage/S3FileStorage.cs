using Amazon.S3;
using Amazon.S3.Model;
using Kamirion.RepairShop.Infrastructure.Storage.Settings;
using Kamirion.RepairShop.Shared.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kamirion.RepairShop.Infrastructure.Storage;

internal sealed class S3FileStorage : IFileStorage
{
    private readonly IAmazonS3 _s3;
    private readonly AwsS3Options _options;
    private readonly ILogger<S3FileStorage> _logger;

    public S3FileStorage(IAmazonS3 s3, IOptions<AwsS3Options> options, ILogger<S3FileStorage> logger)
    {
        _s3 = s3;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> UploadAsync(string key, Stream content, string contentType, CancellationToken ct = default)
    {
        var request = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = key,
            InputStream = content,
            ContentType = contentType,
            AutoCloseStream = false
        };

        await _s3.PutObjectAsync(request, ct);
        _logger.LogInformation("Archivo subido a S3. Key: {Key}", key);
        return key;
    }

    public Task<string> GetPresignedUrlAsync(string key, TimeSpan expiry, CancellationToken ct = default)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _options.BucketName,
            Key = key,
            Expires = DateTime.UtcNow.Add(expiry),
            Protocol = Protocol.HTTPS
        };

        // GetPreSignedURL es síncrono en AWSSDK.S3 v3; no realiza I/O de red
        var url = _s3.GetPreSignedURL(request);
        return Task.FromResult(url);
    }

    public async Task DeleteAsync(string key, CancellationToken ct = default)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _options.BucketName,
            Key = key
        };

        await _s3.DeleteObjectAsync(request, ct);
        _logger.LogInformation("Archivo eliminado de S3. Key: {Key}", key);
    }
}
