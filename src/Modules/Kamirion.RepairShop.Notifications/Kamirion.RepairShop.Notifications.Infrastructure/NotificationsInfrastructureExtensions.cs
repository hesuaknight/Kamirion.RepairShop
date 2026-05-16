using Kamirion.RepairShop.Notifications.Application;
using Kamirion.RepairShop.Notifications.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Kamirion.RepairShop.Notifications.Infrastructure;

public static class NotificationsInfrastructureExtensions
{
    public static IServiceCollection AddNotificationsInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationService, NotificationService>();
        return services;
    }
}
