using Kamirion.RepairShop.Shared.Results;
using MediatR;

namespace Kamirion.RepairShop.Identity.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result>;
