using Kamirion.RepairShop.Shared.Results;

namespace Kamirion.RepairShop.Communication.Contracts;

public interface IWhatsAppSender
{
    Task<Result> SendAsync(
        string toPhoneNumber,
        string contentSid,
        IReadOnlyDictionary<string, string> variables,
        CancellationToken cancellationToken = default);

    Task<Result> SendFreeFormAsync(
        string toPhoneNumber,
        string messageBody,
        CancellationToken cancellationToken = default);
}
