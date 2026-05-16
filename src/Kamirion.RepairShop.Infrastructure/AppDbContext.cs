using System.Reflection;
using Kamirion.RepairShop.Communication.Domain;
using Kamirion.RepairShop.Infrastructure.Identity;
using Kamirion.RepairShop.Notifications.Domain;
using Kamirion.RepairShop.Search.Domain;
using Kamirion.RepairShop.Shared.Abstractions;
using Kamirion.RepairShop.Shared.Domain;
using Kamirion.RepairShop.Shared.Identity;
using Kamirion.RepairShop.Tenancy.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kamirion.RepairShop.Infrastructure;

public class AppDbContext : IdentityDbContext<ApplicationUser>, IDomainEventSource
{
    private readonly ITenantContext _tenantContext;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<SecurityAuditLog> SecurityAuditLogs => Set<SecurityAuditLog>();
    public DbSet<MessageTemplate> MessageTemplates => Set<MessageTemplate>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SearchIndexEntry> SearchIndexEntries => Set<SearchIndexEntry>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        ApplyTenantQueryFilters(builder);
    }

    private void ApplyTenantQueryFilters(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (!typeof(ITenantOwned).IsAssignableFrom(entityType.ClrType))
                continue;

            var method = GetType()
                .GetMethod(nameof(ApplyTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(entityType.ClrType);

            method.Invoke(this, [builder]);
        }
    }

    private void ApplyTenantFilter<TEntity>(ModelBuilder builder) where TEntity : class, ITenantOwned
    {
        builder.Entity<TEntity>().HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
    }

    public IReadOnlyList<IDomainEvent> GetPendingDomainEvents() =>
        ChangeTracker.Entries<AggregateRoot<string>>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

    public void ClearAllDomainEvents()
    {
        foreach (var entry in ChangeTracker.Entries<AggregateRoot<string>>())
            entry.Entity.ClearDomainEvents();
    }
}
