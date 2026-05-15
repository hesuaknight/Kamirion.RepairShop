using System.Text.Json;
using Kamirion.RepairShop.Communication.Contracts;
using Kamirion.RepairShop.Communication.Infrastructure.Settings;
using Kamirion.RepairShop.Shared.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Kamirion.RepairShop.Communication.Infrastructure;

public sealed class TwilioWhatsAppSender : IWhatsAppSender
{
    private readonly TwilioSettings _settings;
    private readonly ILogger<TwilioWhatsAppSender> _logger;

    public TwilioWhatsAppSender(IOptions<TwilioSettings> settings, ILogger<TwilioWhatsAppSender> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<Result> SendAsync(
        string toPhoneNumber,
        string contentSid,
        IReadOnlyDictionary<string, string> variables,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await MessageResource.CreateAsync(
                to: new PhoneNumber($"whatsapp:{toPhoneNumber}"),
                from: new PhoneNumber($"whatsapp:{_settings.FromWhatsAppNumber}"),
                contentSid: contentSid,
                contentVariables: JsonSerializer.Serialize(variables));

            return Result.Success();
        }
        catch (TwilioException ex)
        {
            _logger.LogError(ex, "Twilio error sending template {ContentSid} to {Phone}", contentSid, toPhoneNumber);
            return Result.Failure(new Error("WhatsApp.SendFailed", ex.Message));
        }
    }

    public async Task<Result> SendFreeFormAsync(
        string toPhoneNumber,
        string messageBody,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await MessageResource.CreateAsync(
                to: new PhoneNumber($"whatsapp:{toPhoneNumber}"),
                from: new PhoneNumber($"whatsapp:{_settings.FromWhatsAppNumber}"),
                body: messageBody);

            return Result.Success();
        }
        catch (TwilioException ex)
        {
            _logger.LogError(ex, "Twilio error sending free-form message to {Phone}", toPhoneNumber);
            return Result.Failure(new Error("WhatsApp.SendFailed", ex.Message));
        }
    }
}
