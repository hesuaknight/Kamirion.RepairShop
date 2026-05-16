using Kamirion.RepairShop.Shared.Results;

namespace Kamirion.RepairShop.Shared.Utils;

public static class FileStorageErrors
{
    public static Error InvalidExtension(string ext) =>
        new("FileStorage.InvalidExtension", $"La extensión '{ext}' no está permitida.");

    public static Error ContentTypeMismatch(string contentType, string ext) =>
        new("FileStorage.ContentTypeMismatch", $"El Content-Type '{contentType}' no corresponde a la extensión '{ext}'.");

    public static Error FileTooLarge(long actualBytes, long maxBytes) =>
        new("FileStorage.FileTooLarge", $"El archivo ({actualBytes / (1024 * 1024.0):0.##} MB) supera el límite de {maxBytes / (1024 * 1024)} MB.");

    public static Error UploadFailed(string reason) =>
        new("FileStorage.UploadFailed", $"Error al subir el archivo: {reason}");

    public static Error DeleteFailed(string reason) =>
        new("FileStorage.DeleteFailed", $"Error al eliminar el archivo: {reason}");
}
