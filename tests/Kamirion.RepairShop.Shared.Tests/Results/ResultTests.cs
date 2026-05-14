using FluentAssertions;
using Kamirion.RepairShop.Shared.Results;
using Xunit;

namespace Kamirion.RepairShop.Shared.Tests.Results;

public class ResultTests
{
    [Fact]
    public void Success_WhenCalled_ShouldHaveIsSuccessTrue()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Success_WhenCalled_ShouldHaveIsFailureFalse()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Success_WhenCalled_ShouldHaveErrorNone()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Failure_WhenCalled_ShouldHaveIsFailureTrue()
    {
        // Arrange
        var error = Error.NotFound("Customer");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Failure_WhenCalled_ShouldHaveIsSuccessFalse()
    {
        // Arrange
        var error = Error.NotFound("Customer");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Failure_WhenCalled_ShouldHaveCorrectError()
    {
        // Arrange
        var error = Error.NotFound("Customer");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.Error.Should().Be(error);
    }

    [Fact]
    public void SuccessOfT_WhenCalled_ShouldReturnCorrectValue()
    {
        // Arrange
        const string value = "test-value";

        // Act
        var result = Result.Success(value);

        // Assert
        result.Value.Should().Be(value);
    }

    [Fact]
    public void SuccessOfT_WhenCalled_ShouldHaveIsSuccessTrue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = Result.Success(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void FailureOfT_AccessingValue_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var result = Result.Failure<string>(Error.NotFound("Customer"));

        // Act
        var act = () => result.Value;

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Failure_WithErrorNone_ShouldThrowInvalidOperationException()
    {
        // Arrange & Act
        var act = () => Result.Failure(Error.None);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
