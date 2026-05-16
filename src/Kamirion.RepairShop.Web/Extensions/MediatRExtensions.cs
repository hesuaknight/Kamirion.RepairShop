using System.Reflection;
using FluentValidation;
using Kamirion.RepairShop.Shared.Behaviors;
using MediatR;

namespace Kamirion.RepairShop.Web.Extensions;

internal static class MediatRExtensions
{
    internal static IServiceCollection AddMediatRInfrastructure(this IServiceCollection services)
    {
        // Load all Application assemblies from disk so lazy-loaded ones are included in the scan.
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var applicationAssemblies = Directory
            .GetFiles(baseDir, "Kamirion.RepairShop.*.Application.dll")
            .Select(Assembly.LoadFrom)
            .ToArray();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(applicationAssemblies);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(DomainEventDispatchBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(applicationAssemblies, includeInternalTypes: true);

        return services;
    }
}
