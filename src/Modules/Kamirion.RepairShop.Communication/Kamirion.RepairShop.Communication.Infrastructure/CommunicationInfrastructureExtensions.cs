using Kamirion.RepairShop.Communication.Application.Services;
using Kamirion.RepairShop.Communication.Contracts;
using Kamirion.RepairShop.Communication.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kamirion.RepairShop.Communication.Infrastructure;

public static class CommunicationInfrastructureExtensions
{
    public static IServiceCollection AddCommunicationInfrastructure(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        if (environment.IsDevelopment())
        {
            services.AddScoped<IWhatsAppSender, NullWhatsAppSender>();
        }
        else
        {
            var settings = configuration.GetSection(TwilioSettings.SectionName).Get<TwilioSettings>()
                ?? throw new InvalidOperationException(
                    "Twilio configuration is missing. Ensure Twilio__AccountSid, Twilio__AuthToken, and Twilio__FromWhatsAppNumber are set.");

            TwilioClientInitializer.Initialize(settings.AccountSid, settings.AuthToken);

            services.Configure<TwilioSettings>(configuration.GetSection(TwilioSettings.SectionName));
            services.AddScoped<IWhatsAppSender, TwilioWhatsAppSender>();
        }

        services.AddScoped<IMessageTemplateRepository, MessageTemplateRepository>();
        services.AddScoped<IMessageTemplateService, MessageTemplateService>();

        return services;
    }
}
