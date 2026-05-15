using FluentValidation;
using Kamirion.RepairShop.Shared.Behaviors;
using MediatR;

namespace Kamirion.RepairShop.Web.Extensions;

internal static class MediatRExtensions
{
    internal static IServiceCollection AddMediatRInfrastructure(this IServiceCollection services)
    {
        // All Application assemblies are referenced by this project and loaded at startup.
        var applicationAssemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => a.FullName?.Contains("Kamirion.RepairShop.") == true
                     && a.FullName.Contains(".Application,"))
            .ToArray();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(applicationAssemblies);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(applicationAssemblies, includeInternalTypes: true);

        return services;
    }
}
