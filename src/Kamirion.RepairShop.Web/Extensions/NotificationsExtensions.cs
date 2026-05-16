using Kamirion.RepairShop.Notifications.Infrastructure;

namespace Kamirion.RepairShop.Web.Extensions;

internal static class NotificationsExtensions
{
    internal static IServiceCollection AddNotifications(this IServiceCollection services)
        => services.AddNotificationsInfrastructure();
}
