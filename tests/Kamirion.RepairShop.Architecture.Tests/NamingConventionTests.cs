using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Kamirion.RepairShop.Shared.Domain;
using NetArchTest.Rules;
using Xunit;

namespace Kamirion.RepairShop.Architecture.Tests;

public class NamingConventionTests
{
    private static readonly string[] ModuleNames =
    [
        "Analytics", "Audit", "Automation", "Communication", "Configuration",
        "Customers", "Devices", "Identity", "Inventory", "Notifications",
        "Payments", "RepairWorkflow", "Search", "Tenancy"
    ];

    private static Assembly GetAssembly(string name) =>
        AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == name)
        ?? Assembly.Load(name);

    private static IReadOnlyList<Assembly> GetDomainAssemblies() =>
        ModuleNames.Select(m => GetAssembly($"Kamirion.RepairShop.{m}.Domain")).ToList();

    // Recorre la jerarquía de tipos verificando si algún ancestro es el tipo genérico abierto dado
    private static bool IsAssignableToGenericType(Type type, Type genericType)
    {
        var t = type;
        while (t != null && t != typeof(object))
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == genericType)
                return true;
            t = t.BaseType;
        }
        return false;
    }

    [Fact]
    public void DomainEntities_ShouldInherit_EntityOrAggregateRoot()
    {
        var domainAssemblies = GetDomainAssemblies();

        // Heurística: clase concreta, no-evento, con propiedad Id → es una entidad
        var violations = domainAssemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition)
            .Where(t => !typeof(IDomainEvent).IsAssignableFrom(t))
            .Where(t => t.GetProperty("Id") != null)
            .Where(t => !IsAssignableToGenericType(t, typeof(Entity<>)) &&
                        !IsAssignableToGenericType(t, typeof(AggregateRoot<>)))
            .ToList();

        violations.Should().BeEmpty(
            because: $"all domain entities must inherit from Entity<> or AggregateRoot<>. " +
                     $"Violations: {string.Join(", ", violations.Select(t => t.FullName))}");
    }

    [Fact]
    public void DomainEvents_ShouldImplement_IDomainEvent()
    {
        var domainAssemblies = GetDomainAssemblies();

        var result = Types.InAssemblies(domainAssemblies)
            .That()
            .AreClasses()
            .And().AreNotAbstract()
            .And().HaveNameEndingWith("Event")
            .Should()
            .ImplementInterface(typeof(IDomainEvent))
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            because: "all domain event classes must implement IDomainEvent");
    }
}
