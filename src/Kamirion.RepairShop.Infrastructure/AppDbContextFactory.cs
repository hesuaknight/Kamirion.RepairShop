using Kamirion.RepairShop.Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kamirion.RepairShop.Infrastructure;

// Solo usado por EF Core tools en design-time (dotnet ef migrations)
internal sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RepairShopDesignTime;Trusted_Connection=True;");

        return new AppDbContext(optionsBuilder.Options, new TenantContext());
    }
}
