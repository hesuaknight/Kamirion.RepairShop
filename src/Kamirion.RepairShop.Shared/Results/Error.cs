namespace Kamirion.RepairShop.Shared.Results;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error NotFound(string entity) => new($"{entity}.NotFound", $"{entity} was not found.");

    public static Error Validation(string field, string message) => new($"Validation.{field}", message);

    public static Error Unauthorized() => new("Auth.Unauthorized", "Unauthorized.");

    public static Error Conflict(string message) => new("Conflict", message);
}
