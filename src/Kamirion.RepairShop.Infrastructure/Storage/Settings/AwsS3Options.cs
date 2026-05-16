namespace Kamirion.RepairShop.Infrastructure.Storage.Settings;

public sealed class AwsS3Options
{
    public const string SectionName = "AwsS3";

    public string BucketName { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;

    // Dejar vacíos en producción sobre EC2/ECS para usar el IAM role de la instancia
    public string AccessKeyId { get; init; } = string.Empty;
    public string SecretAccessKey { get; init; } = string.Empty;
}
