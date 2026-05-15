namespace Kamirion.RepairShop.Shared.Realtime;

public static class RealtimeGroups
{
    public static string Tenant(string tenantId) => $"tenant:{tenantId}";
    public static string Branch(string tenantId, string branchId) => $"branch:{tenantId}:{branchId}";
    public static string Board(string tenantId, string branchId) => $"board:{tenantId}:{branchId}";
    public static string User(string userId) => $"user:{userId}";
}
