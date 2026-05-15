using Kamirion.RepairShop.Shared.Abstractions;
using Kamirion.RepairShop.Shared.Domain;
using Kamirion.RepairShop.Shared.Results;
using Kamirion.RepairShop.Shared.Utils;


namespace Kamirion.RepairShop.Communication.Domain;

public sealed class MessageTemplate : Entity<string>, ITenantOwned
{
    public string TenantId { get; private set; } = string.Empty;
    public string TemplateKey { get; private set; } = string.Empty;
    public string Channel { get; private set; } = string.Empty;
    public string Culture { get; private set; } = string.Empty;
    public string? TwilioContentSid { get; private set; }
    public string BodyTemplate { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private MessageTemplate() { }

    public static Result<MessageTemplate> Create(
        string tenantId,
        string templateKey,
        string channel,
        string culture,
        string bodyTemplate,
        string? twilioContentSid = null)
    {
        if (string.IsNullOrWhiteSpace(tenantId))
            return Result.Failure<MessageTemplate>(Error.Validation(nameof(TenantId), "MessageTemplate_TenantId_Required"));

        if (string.IsNullOrWhiteSpace(templateKey))
            return Result.Failure<MessageTemplate>(Error.Validation(nameof(TemplateKey), "MessageTemplate_TemplateKey_Required"));

        if (string.IsNullOrWhiteSpace(channel))
            return Result.Failure<MessageTemplate>(Error.Validation(nameof(Channel), "MessageTemplate_Channel_Required"));

        if (string.IsNullOrWhiteSpace(culture))
            return Result.Failure<MessageTemplate>(Error.Validation(nameof(Culture), "MessageTemplate_Culture_Required"));

        if (string.IsNullOrWhiteSpace(bodyTemplate))
            return Result.Failure<MessageTemplate>(Error.Validation(nameof(BodyTemplate), "MessageTemplate_BodyTemplate_Required"));

        return Result.Success(new MessageTemplate
        {
            Id = UlidGenerator.New(),
            TenantId = tenantId,
            TemplateKey = templateKey,
            Channel = channel,
            Culture = culture,
            TwilioContentSid = twilioContentSid,
            BodyTemplate = bodyTemplate,
            IsActive = true
        });
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
