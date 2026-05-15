using Kamirion.RepairShop.Identity.Application.Services;
using Kamirion.RepairShop.Shared.Results;
using MediatR;

namespace Kamirion.RepairShop.Identity.Application.Commands.Logout;

internal sealed class LogoutCommandHandler(IAuthenticationService authenticationService)
    : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await authenticationService.SignOutAsync(cancellationToken);
        return Result.Success();
    }
}
