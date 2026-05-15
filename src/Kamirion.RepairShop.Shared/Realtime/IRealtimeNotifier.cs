namespace Kamirion.RepairShop.Shared.Realtime;

public interface IRealtimeNotifier
{
    Task NotifyTenantAsync(string tenantId, string method, object? payload, CancellationToken ct = default);
    Task NotifyUserAsync(string userId, string method, object? payload, CancellationToken ct = default);
    Task NotifyGroupAsync(string groupName, string method, object? payload, CancellationToken ct = default);
}
