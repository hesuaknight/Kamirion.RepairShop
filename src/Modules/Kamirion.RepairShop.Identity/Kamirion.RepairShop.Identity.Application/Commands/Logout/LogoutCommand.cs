using Kamirion.RepairShop.Shared.Results;
using MediatR;

namespace Kamirion.RepairShop.Identity.Application.Commands.Logout;

public record LogoutCommand : IRequest<Result>;
