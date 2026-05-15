using Kamirion.RepairShop.Communication.Contracts;
using Kamirion.RepairShop.Shared.Results;
using Microsoft.Extensions.Logging;

namespace Kamirion.RepairShop.Communication.Infrastructure;

public sealed class NullWhatsAppSender : IWhatsAppSender
{
    private readonly ILogger<NullWhatsAppSender> _logger;

    public NullWhatsAppSender(ILogger<NullWhatsAppSender> logger)
    {
        _logger = logger;
    }

    public Task<Result> SendAsync(
        string toPhoneNumber,
        string contentSid,
        IReadOnlyDictionary<string, string> variables,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[NullWhatsAppSender] Template {ContentSid} → {Phone} | vars: {Variables}",
            contentSid, toPhoneNumber, variables);

        return Task.FromResult(Result.Success());
    }

    public Task<Result> SendFreeFormAsync(
        string toPhoneNumber,
        string messageBody,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[NullWhatsAppSender] Free-form → {Phone} | body: {Body}",
            toPhoneNumber, messageBody);

        return Task.FromResult(Result.Success());
    }
}
