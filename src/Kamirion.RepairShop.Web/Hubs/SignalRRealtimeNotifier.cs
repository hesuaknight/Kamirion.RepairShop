using Kamirion.RepairShop.Shared.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace Kamirion.RepairShop.Web.Hubs;

// Placed in Web (not Infrastructure) because IHubContext<OperationalHub> requires
// a reference to OperationalHub, which lives in Web. Infrastructure cannot reference
// Web without creating a circular dependency.
internal sealed class SignalRRealtimeNotifier(IHubContext<OperationalHub> hubContext) : IRealtimeNotifier
{
    public Task NotifyTenantAsync(string tenantId, string method, object? payload, CancellationToken ct = default)
        => hubContext.Clients.Group(RealtimeGroups.Tenant(tenantId)).SendAsync(method, payload, ct);

    public Task NotifyUserAsync(string userId, string method, object? payload, CancellationToken ct = default)
        => hubContext.Clients.Group(RealtimeGroups.User(userId)).SendAsync(method, payload, ct);

    public Task NotifyGroupAsync(string groupName, string method, object? payload, CancellationToken ct = default)
        => hubContext.Clients.Group(groupName).SendAsync(method, payload, ct);
}
