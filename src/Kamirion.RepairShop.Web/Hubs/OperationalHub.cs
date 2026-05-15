using Kamirion.RepairShop.Shared.Identity;
using Kamirion.RepairShop.Shared.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Kamirion.RepairShop.Web.Hubs;

[Authorize]
public sealed class OperationalHub : Hub
{
    private const string TenantIdKey = "TenantId";

    // OnConnectedAsync runs within the WebSocket upgrade request's DI scope,
    // where TenantResolutionMiddleware has already resolved ITenantContext.
    // We capture TenantId into Context.Items (per-connection, outside DI) so
    // that JoinGroup/LeaveGroup can read it from their independent DI scopes.
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext is not null)
        {
            var tenantContext = httpContext.RequestServices.GetRequiredService<ITenantContext>();
            if (tenantContext.IsResolved)
            {
                Context.Items[TenantIdKey] = tenantContext.TenantId;
                await Groups.AddToGroupAsync(Context.ConnectionId, RealtimeGroups.Tenant(tenantContext.TenantId));
            }

            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, RealtimeGroups.User(userId));
            }
        }

        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
        => base.OnDisconnectedAsync(exception);

    public async Task JoinGroup(string groupName)
    {
        if (!IsGroupOwnedByConnectionTenant(groupName))
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        if (!IsGroupOwnedByConnectionTenant(groupName))
        {
            Context.Abort();
            return;
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    private bool IsGroupOwnedByConnectionTenant(string groupName)
    {
        if (!Context.Items.TryGetValue(TenantIdKey, out var raw) || raw is not string tenantId)
            return false;

        // All valid tenant-scoped groups embed tenantId as the second colon-delimited segment:
        //   tenant:{tenantId}
        //   branch:{tenantId}:{branchId}
        //   board:{tenantId}:{branchId}
        // user:{userId} groups are assigned automatically on connect and not accessible via JoinGroup.
        var parts = groupName.Split(':');
        return parts.Length >= 2 && parts[1] == tenantId;
    }
}
