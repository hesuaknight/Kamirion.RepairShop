using Amazon;
using Amazon.S3;
using Kamirion.RepairShop.Infrastructure.Storage;
using Kamirion.RepairShop.Infrastructure.Storage.Settings;
using Kamirion.RepairShop.Shared.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kamirion.RepairShop.Infrastructure.Extensions;

public static class FileStorageExtensions
{
    public static IServiceCollection AddFileStorage(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        if (environment.IsDevelopment())
        {
            services.AddScoped<IFileStorage, LocalFileStorage>();
        }
        else
        {
            services.Configure<AwsS3Options>(configuration.GetSection(AwsS3Options.SectionName));

            var options = configuration.GetSection(AwsS3Options.SectionName).Get<AwsS3Options>()
                ?? throw new InvalidOperationException(
                    "La configuración de AWS S3 está ausente. Verificar AwsS3__BucketName y AwsS3__Region.");

            services.AddSingleton<IAmazonS3>(_ =>
            {
                var region = RegionEndpoint.GetBySystemName(options.Region);

                return string.IsNullOrWhiteSpace(options.AccessKeyId)
                    ? new AmazonS3Client(region)
                    : new AmazonS3Client(options.AccessKeyId, options.SecretAccessKey, region);
            });

            services.AddScoped<IFileStorage, S3FileStorage>();
        }

        return services;
    }
}
