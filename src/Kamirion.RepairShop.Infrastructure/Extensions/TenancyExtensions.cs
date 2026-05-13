using Kamirion.RepairShop.Infrastructure.Tenancy;
using Kamirion.RepairShop.Shared.Identity;
using Kamirion.RepairShop.Tenancy.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Kamirion.RepairShop.Infrastructure.Extensions;

public static class TenancyExtensions
{
    public static IServiceCollection AddTenancy(this IServiceCollection services)
    {
        services.AddScoped<TenantContext>();
        services.AddScoped<ITenantContext>(sp => sp.GetRequiredService<TenantContext>());
        services.AddScoped<ITenantLookupService, TenantLookupService>();
        return services;
    }
}
