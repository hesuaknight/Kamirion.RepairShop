using Kamirion.RepairShop.Shared.Results;

namespace Kamirion.RepairShop.Communication.Contracts;

public interface IMessageTemplateService
{
    Task<Result<ResolvedMessage>> ResolveAsync(
        string tenantId,
        string templateKey,
        string channel,
        string culture,
        IReadOnlyDictionary<string, string>? variables = null,
        CancellationToken cancellationToken = default);
}
