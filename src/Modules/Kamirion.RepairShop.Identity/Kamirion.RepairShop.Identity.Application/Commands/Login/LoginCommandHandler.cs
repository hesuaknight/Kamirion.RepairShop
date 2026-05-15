using Kamirion.RepairShop.Identity.Application.Services;
using Kamirion.RepairShop.Shared.Results;
using MediatR;

namespace Kamirion.RepairShop.Identity.Application.Commands.Login;

internal sealed class LoginCommandHandler(IAuthenticationService authenticationService)
    : IRequestHandler<LoginCommand, Result>
{
    public Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken) =>
        authenticationService.SignInAsync(request.Email, request.Password, cancellationToken);
}
