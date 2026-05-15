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

    builder.Services.AddControllersWithViews();
    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddTenancy();
    builder.Services.AddAppIdentity();
    builder.Services.AddHangfireInfrastructure(builder.Configuration);
    builder.Services.AddMediatRInfrastructure();
    builder.AddSerilog();

    var app = builder.Build();

    await using (var scope = app.Services.CreateAsyncScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
        Log.Information("Database migrations applied successfully");
    }

    app.UseMiddleware<GlobalExceptionMiddleware>();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.UseRouting();
    app.UseMiddleware<TenantResolutionMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHangfireInfrastructure();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

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
