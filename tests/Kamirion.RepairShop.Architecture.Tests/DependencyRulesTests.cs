using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace Kamirion.RepairShop.Architecture.Tests;

public class DependencyRulesTests
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

    private static IReadOnlyList<Assembly> GetLayerAssemblies(string layer) =>
        ModuleNames.Select(m => GetAssembly($"Kamirion.RepairShop.{m}.{layer}")).ToList();

    private static string[] GetAllInfrastructureNamespaces() =>
        ModuleNames
            .Select(m => $"Kamirion.RepairShop.{m}.Infrastructure")
            .Append("Kamirion.RepairShop.Infrastructure")
            .ToArray();

    [Fact]
    public void Domain_ShouldNot_HaveDependencyOn_Infrastructure()
    {
        var domainAssemblies = GetLayerAssemblies("Domain");
        var infraNamespaces = GetAllInfrastructureNamespaces();

        var result = Types.InAssemblies(domainAssemblies)
            .Should()
            .NotHaveDependencyOnAny(infraNamespaces)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            because: "Domain layer must not depend on any Infrastructure layer");
    }

    [Fact]
    public void Domain_ShouldNot_HaveDependencyOn_Web()
    {
        var domainAssemblies = GetLayerAssemblies("Domain");

        var result = Types.InAssemblies(domainAssemblies)
            .Should()
            .NotHaveDependencyOn("Kamirion.RepairShop.Web")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            because: "Domain layer must not depend on the Web layer");
    }

    [Fact]
    public void Application_ShouldNot_HaveDependencyOn_Infrastructure()
    {
        var applicationAssemblies = GetLayerAssemblies("Application");
        var infraNamespaces = GetAllInfrastructureNamespaces();

        var result = Types.InAssemblies(applicationAssemblies)
            .Should()
            .NotHaveDependencyOnAny(infraNamespaces)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            because: "Application layer must not depend on any Infrastructure layer");
    }

    [Fact]
    public void Modules_ShouldNot_HaveDirectCrossModuleDependencies()
    {
        foreach (var module in ModuleNames)
        {
            var allLayerAssemblies = new[] { "Domain", "Application", "Infrastructure", "Contracts" }
                .Select(l => GetAssembly($"Kamirion.RepairShop.{module}.{l}"))
                .ToArray();

            var forbiddenNamespaces = ModuleNames
                .Where(m => m != module)
                .SelectMany(m => new[]
                {
                    $"Kamirion.RepairShop.{m}.Domain",
                    $"Kamirion.RepairShop.{m}.Application",
                    $"Kamirion.RepairShop.{m}.Infrastructure",
                })
                .ToArray();

            var result = Types.InAssemblies(allLayerAssemblies)
                .Should()
                .NotHaveDependencyOnAny(forbiddenNamespaces)
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: $"module '{module}' should only communicate with other modules via Contracts");
        }
    }
}
