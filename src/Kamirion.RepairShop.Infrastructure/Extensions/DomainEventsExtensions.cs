using Kamirion.RepairShop.Infrastructure.Domain;
using Kamirion.RepairShop.Shared.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Kamirion.RepairShop.Infrastructure.Extensions;

public static class DomainEventsExtensions
{
    public static IServiceCollection AddDomainEvents(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
        services.AddScoped<IDomainEventSource>(sp => sp.GetRequiredService<AppDbContext>());
        return services;
    }
}
