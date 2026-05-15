using Kamirion.RepairShop.Identity.Application.Services;
using Kamirion.RepairShop.Identity.Contracts;
using Kamirion.RepairShop.Infrastructure.Identity;
using Kamirion.RepairShop.Shared.Results;
using Microsoft.AspNetCore.Identity;

namespace Kamirion.RepairShop.Identity.Infrastructure.Services;

internal sealed class AuthenticationService(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    ISecurityAuditService securityAudit) : IAuthenticationService
{
    public async Task<Result> SignInAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            await securityAudit.RecordAsync(
                SecurityEventTypes.LoginFailed,
                success: false,
                userEmail: email,
                cancellationToken: cancellationToken);

            return Result.Failure(Error.Unauthorized());
        }

        var signInResult = await signInManager.PasswordSignInAsync(
            user, password, isPersistent: false, lockoutOnFailure: true);

        var eventType = signInResult.Succeeded ? SecurityEventTypes.LoginSuccess : SecurityEventTypes.LoginFailed;

        await securityAudit.RecordAsync(
            eventType,
            success: signInResult.Succeeded,
            tenantId: user.TenantId,
            userId: user.Id,
            userEmail: email,
            cancellationToken: cancellationToken);

        return signInResult.Succeeded
            ? Result.Success()
            : Result.Failure(Error.Unauthorized());
    }

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        await securityAudit.RecordAsync(
            SecurityEventTypes.Logout,
            success: true,
            cancellationToken: cancellationToken);

        await signInManager.SignOutAsync();
    }
}
