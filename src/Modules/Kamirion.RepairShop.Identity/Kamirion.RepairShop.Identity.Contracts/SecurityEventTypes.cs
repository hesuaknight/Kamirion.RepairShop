namespace Kamirion.RepairShop.Identity.Contracts;

public static class SecurityEventTypes
{
    public const string LoginSuccess = "Login.Success";
    public const string LoginFailed = "Login.Failed";
    public const string Logout = "Logout";
    public const string AccessDenied = "Access.Denied";
    public const string AuthenticatedRequest = "Request.Authenticated";
    public const string SensitiveDataAccess = "Data.SensitiveAccess";
}
