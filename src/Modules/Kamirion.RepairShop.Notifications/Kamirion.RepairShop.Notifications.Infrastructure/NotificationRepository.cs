using Kamirion.RepairShop.Infrastructure;
using Kamirion.RepairShop.Notifications.Application;
using Kamirion.RepairShop.Notifications.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kamirion.RepairShop.Notifications.Infrastructure;

internal sealed class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Notification?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Notification>> GetUnreadAsync(
        string tenantId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .Where(n => n.TenantId == tenantId && n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
