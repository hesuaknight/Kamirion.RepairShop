using Serilog;
using Serilog.Events;
using Serilog.Sinks.Discord;

namespace Kamirion.RepairShop.Web.Extensions;

public static class SerilogExtensions
{
    public static void ConfigureBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .WriteTo.Console(outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
            .WriteTo.File("logs/repairshop-.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            .CreateLogger();
    }

    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, cfg) =>
        {
            cfg
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Application", "Kamirion.RepairShop")
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .WriteTo.File("logs/repairshop-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7);

            if (ctx.HostingEnvironment.IsDevelopment())
                cfg.WriteTo.Seq("http://localhost:5341");

            if (ctx.HostingEnvironment.IsProduction())
            {
                var webhookId = ulong.Parse(Environment.GetEnvironmentVariable("Discord__WebhookId") ?? "0");
                var webhookToken = Environment.GetEnvironmentVariable("Discord__WebhookToken") ?? string.Empty;
                if (webhookId != 0 && !string.IsNullOrEmpty(webhookToken))
                    cfg.WriteTo.Discord(webhookId, webhookToken, null,
                        restrictedToMinimumLevel: LogEventLevel.Error);
            }
        });

        return builder;
    }
}
