using Kamirion.RepairShop.Shared.Results;

namespace Kamirion.RepairShop.Identity.Application.Services;

public interface IAuthenticationService
{
    Task<Result> SignInAsync(string email, string password, CancellationToken cancellationToken = default);
    Task SignOutAsync(CancellationToken cancellationToken = default);
}
