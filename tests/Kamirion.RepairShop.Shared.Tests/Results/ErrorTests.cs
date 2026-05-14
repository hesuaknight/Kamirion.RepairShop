using FluentAssertions;
using Kamirion.RepairShop.Shared.Results;
using Xunit;

namespace Kamirion.RepairShop.Shared.Tests.Results;

public class ErrorTests
{
    [Fact]
    public void None_ShouldHaveEmptyCode()
    {
        // Arrange & Act
        var error = Error.None;

        // Assert
        error.Code.Should().BeEmpty();
    }

    [Fact]
    public void None_ShouldHaveEmptyDescription()
    {
        // Arrange & Act
        var error = Error.None;

        // Assert
        error.Description.Should().BeEmpty();
    }

    [Fact]
    public void NotFound_WhenEntityProvided_ShouldGenerateCorrectCode()
    {
        // Arrange & Act
        var error = Error.NotFound("Customer");

        // Assert
        error.Code.Should().Be("Customer.NotFound");
    }

    [Fact]
    public void NotFound_WhenEntityProvided_ShouldGenerateNonEmptyDescription()
    {
        // Arrange & Act
        var error = Error.NotFound("Customer");

        // Assert
        error.Description.Should().Contain("Customer");
    }
}
