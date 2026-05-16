using Kamirion.RepairShop.Search.Infrastructure;

namespace Kamirion.RepairShop.Web.Extensions;

internal static class SearchExtensions
{
    internal static IServiceCollection AddSearch(this IServiceCollection services)
        => services.AddSearchInfrastructure();
}
