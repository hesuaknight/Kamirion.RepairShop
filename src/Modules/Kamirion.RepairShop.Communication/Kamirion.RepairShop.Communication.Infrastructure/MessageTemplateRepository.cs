using Kamirion.RepairShop.Communication.Contracts;
using Kamirion.RepairShop.Communication.Domain;
using Kamirion.RepairShop.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Kamirion.RepairShop.Communication.Infrastructure;

internal sealed class MessageTemplateRepository : IMessageTemplateRepository
{
    private readonly AppDbContext _context;

    public MessageTemplateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MessageTemplate?> FindAsync(
        string tenantId,
        string templateKey,
        string channel,
        string culture,
        CancellationToken cancellationToken = default)
    {
        return await _context.MessageTemplates
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(t =>
                t.TenantId == tenantId &&
                t.TemplateKey == templateKey &&
                t.Channel == channel &&
                t.Culture == culture &&
                t.IsActive,
            cancellationToken);
    }
}
