using Kamirion.RepairShop.Communication.Domain;

namespace Kamirion.RepairShop.Communication.Contracts;

public interface IMessageTemplateRepository
{
    Task<MessageTemplate?> FindAsync(
        string tenantId,
        string templateKey,
        string channel,
        string culture,
        CancellationToken cancellationToken = default);
}
