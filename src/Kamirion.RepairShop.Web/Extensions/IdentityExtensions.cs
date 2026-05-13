using Kamirion.RepairShop.Infrastructure;
using Kamirion.RepairShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Kamirion.RepairShop.Web.Extensions;

internal static class IdentityExtensions
{
    internal static IServiceCollection AddAppIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
