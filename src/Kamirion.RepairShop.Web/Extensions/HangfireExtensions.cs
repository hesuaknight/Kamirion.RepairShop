using Hangfire;
using Hangfire.Dashboard;

namespace Kamirion.RepairShop.Web.Extensions;

internal static class HangfireExtensions
{
    internal static IServiceCollection AddHangfireInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

        services.AddHangfireServer(options =>
        {
            options.WorkerCount = 5;
            options.Queues = ["critical", "default", "low"];
        });

        return services;
    }

    internal static WebApplication UseHangfireInfrastructure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = [new AllowAllConnectionsFilter()]
            });
        }

        return app;
    }

    private sealed class AllowAllConnectionsFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
