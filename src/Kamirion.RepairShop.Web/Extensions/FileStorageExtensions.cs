namespace Kamirion.RepairShop.Web.Extensions;

internal static class FileStorageExtensions
{
    internal static WebApplication UseLocalFileStorage(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // Sirve wwwroot/* en runtime, incluye el directorio uploads/ creado dinámicamente
            app.UseStaticFiles();
        }

        return app;
    }
}
