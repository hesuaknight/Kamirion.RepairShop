using Kamirion.RepairShop.Shared.Results;

namespace Kamirion.RepairShop.Shared.Utils;

public static class FileUploadValidator
{
    public const long MaxFileSizeBytes = 10L * 1024 * 1024;

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif", ".pdf"
    };

    private static readonly Dictionary<string, HashSet<string>> ContentTypeToExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["image/jpeg"]       = [".jpg", ".jpeg"],
            ["image/png"]        = [".png"],
            ["image/webp"]       = [".webp"],
            ["image/gif"]        = [".gif"],
            ["application/pdf"]  = [".pdf"],
        };

    public static Result Validate(string fileName, string contentType, long fileSizeBytes)
    {
        var ext = Path.GetExtension(fileName);

        if (string.IsNullOrWhiteSpace(ext) || !AllowedExtensions.Contains(ext))
            return Result.Failure(FileStorageErrors.InvalidExtension(ext));

        if (!ContentTypeToExtensions.TryGetValue(contentType, out var validExts) ||
            !validExts.Contains(ext))
            return Result.Failure(FileStorageErrors.ContentTypeMismatch(contentType, ext));

        if (fileSizeBytes > MaxFileSizeBytes)
            return Result.Failure(FileStorageErrors.FileTooLarge(fileSizeBytes, MaxFileSizeBytes));

        return Result.Success();
    }
}
