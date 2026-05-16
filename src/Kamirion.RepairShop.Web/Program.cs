using Kamirion.RepairShop.Communication.Infrastructure;
using Kamirion.RepairShop.Identity.Infrastructure;
using Kamirion.RepairShop.Infrastructure;
using Kamirion.RepairShop.Infrastructure.Extensions;
using Kamirion.RepairShop.Web.Extensions;
using Kamirion.RepairShop.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;

SerilogExtensions.ConfigureBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages().AddRazorPagesOptions(opts =>
    {
        opts.Conventions.AddAreaPageRoute("Auth", "/Login", "/auth/login");
        opts.Conventions.AddAreaPageRoute("Auth", "/Logout", "/auth/logout");
        opts.Conventions.AddAreaPageRoute("Auth", "/AccessDenied", "/auth/access-denied");
    });
    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddDomainEvents();
    builder.Services.AddTenancy();
    builder.Services.AddAppIdentity();
    builder.Services.AddHangfireInfrastructure(builder.Configuration);
    builder.Services.AddSignalRInfrastructure();
    builder.Services.AddIdentityInfrastructure();
    builder.Services.AddCommunicationInfrastructure(builder.Environment, builder.Configuration);
    builder.Services.AddFileStorage(builder.Environment, builder.Configuration);
    builder.Services.AddMediatRInfrastructure();
    builder.AddSerilog();

    var app = builder.Build();

    await using (var scope = app.Services.CreateAsyncScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
        Log.Information("Database migrations applied successfully");

        if (app.Environment.IsDevelopment())
        {
            await DatabaseSeeder.SeedDevelopmentDataAsync(db);
            Log.Information("Development seed data applied successfully");
        }
    }

    app.UseMiddleware<GlobalExceptionMiddleware>();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseLocalFileStorage();
    app.UseSerilogRequestLogging();
    app.UseRequestLocalization(opts =>
    {
        var supportedCultures = new[] { "es", "es-AR", "en", "en-US" };
        opts.SetDefaultCulture("es")
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
    });
    app.UseRouting();
    app.UseMiddleware<TenantResolutionMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<SecurityAuditMiddleware>();
    app.UseHangfireInfrastructure();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapRazorPages();
    app.UseSignalRInfrastructure();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
