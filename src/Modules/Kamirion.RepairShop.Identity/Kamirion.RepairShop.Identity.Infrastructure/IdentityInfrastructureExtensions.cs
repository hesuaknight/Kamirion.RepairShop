using Kamirion.RepairShop.Identity.Application.Services;
using Kamirion.RepairShop.Identity.Contracts;
using Kamirion.RepairShop.Identity.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Kamirion.RepairShop.Identity.Infrastructure;

public static class IdentityInfrastructureExtensions
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISecurityAuditService, SecurityAuditService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        return services;
    }
}
