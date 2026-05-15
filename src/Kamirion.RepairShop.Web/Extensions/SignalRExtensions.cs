using Kamirion.RepairShop.Shared.Realtime;
using Kamirion.RepairShop.Web.Hubs;

namespace Kamirion.RepairShop.Web.Extensions;

internal static class SignalRExtensions
{
    internal static IServiceCollection AddSignalRInfrastructure(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddScoped<IRealtimeNotifier, SignalRRealtimeNotifier>();
        return services;
    }

    internal static WebApplication UseSignalRInfrastructure(this WebApplication app)
    {
        app.MapHub<OperationalHub>("/hubs/operational");
        return app;
    }
}
