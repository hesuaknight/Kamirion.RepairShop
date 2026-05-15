using Kamirion.RepairShop.Communication.Contracts;
using Kamirion.RepairShop.Shared.Results;

namespace Kamirion.RepairShop.Communication.Application.Services;

public sealed class MessageTemplateService : IMessageTemplateService
{
    private readonly IMessageTemplateRepository _repository;

    public MessageTemplateService(IMessageTemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ResolvedMessage>> ResolveAsync(
        string tenantId,
        string templateKey,
        string channel,
        string culture,
        IReadOnlyDictionary<string, string>? variables = null,
        CancellationToken cancellationToken = default)
    {
        Domain.MessageTemplate? template = null;

        foreach (var candidate in BuildCultureFallback(culture))
        {
            template = await _repository.FindAsync(tenantId, templateKey, channel, candidate, cancellationToken);
            if (template is not null) break;
        }

        if (template is null)
            return Result.Failure<ResolvedMessage>(Error.NotFound("MessageTemplate"));

        var body = template.BodyTemplate;

        // Solo sustituimos variables en el body cuando no hay ContentSid (free-form).
        // Para templates de Twilio, la sustitución la maneja Twilio en su lado.
        if (template.TwilioContentSid is null && variables is not null)
        {
            foreach (var (key, value) in variables)
                body = body.Replace($"{{{{{key}}}}}", value);
        }

        return Result.Success(new ResolvedMessage(template.TwilioContentSid, body));
    }

    private static IEnumerable<string> BuildCultureFallback(string culture)
    {
        yield return culture;

        var dashIndex = culture.IndexOf('-');
        if (dashIndex > 0)
            yield return culture[..dashIndex];

        if (culture != "en")
            yield return "en";
    }
}
