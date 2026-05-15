using Kamirion.RepairShop.Identity.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Kamirion.RepairShop.Identity.Infrastructure;

public static class IdentityInfrastructureExtensions
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISecurityAuditService, SecurityAuditService>();
        return services;
    }
}
