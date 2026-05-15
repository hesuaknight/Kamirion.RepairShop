namespace Kamirion.RepairShop.Communication.Infrastructure.Settings;

public sealed class TwilioSettings
{
    public const string SectionName = "Twilio";

    public string AccountSid { get; init; } = string.Empty;
    public string AuthToken { get; init; } = string.Empty;
    public string FromWhatsAppNumber { get; init; } = string.Empty;
}
