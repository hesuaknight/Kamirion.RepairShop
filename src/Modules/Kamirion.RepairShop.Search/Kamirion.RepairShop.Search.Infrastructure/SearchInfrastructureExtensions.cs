using Kamirion.RepairShop.Search.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Kamirion.RepairShop.Search.Infrastructure;

public static class SearchInfrastructureExtensions
{
    public static IServiceCollection AddSearchInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISearchIndexRepository, SqlSearchIndexRepository>();
        services.AddScoped<ISearchIndexService, SqlSearchIndexService>();
        return services;
    }
}
