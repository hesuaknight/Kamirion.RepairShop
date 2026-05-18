using Microsoft.AspNetCore.Localization;

namespace Kamirion.RepairShop.Web.Extensions;

internal static class LocalizationExtensions
{
    internal static IServiceCollection AddLocalizationInfrastructure(this IServiceCollection services)
    {
        services.AddLocalization();
        return services;
    }

    internal static WebApplication UseLocalizationInfrastructure(this WebApplication app)
    {
        var supportedCultures = new[] { "es", "es-AR", "es-CL", "es-MX", "en", "en-US" };
        app.UseRequestLocalization(opts =>
        {
            opts.SetDefaultCulture("es")
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            opts.RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()];
        });
        return app;
    }
}
