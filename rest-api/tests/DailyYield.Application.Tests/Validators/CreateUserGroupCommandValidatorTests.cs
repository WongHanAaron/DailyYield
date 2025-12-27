using DailyYield.Application.Commands;
using DailyYield.Application.Validators;
using FluentAssertions;
using Xunit;

namespace DailyYield.Application.Tests.Validators;

public class CreateUserGroupCommandValidatorTests
{
    [Fact]
    public void Validate_ValidCommand_ShouldPass()
    {
        // Arrange
        var sut = new CreateUserGroupCommandValidator();
        var command = new CreateUserGroupCommand
        {
            Name = "Valid Group",
            Timezone = "UTC",
            OwnerId = Guid.NewGuid()
        };

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_NameIsEmpty_ShouldFail(string? name)
    {
        // Arrange
        var sut = new CreateUserGroupCommandValidator();
        var command = new CreateUserGroupCommand
        {
            Name = name!,
            Timezone = "UTC",
            OwnerId = Guid.NewGuid()
        };

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Name" &&
            e.ErrorMessage == "User group name is required");
    }

    [Fact]
    public void Validate_NameTooShort_ShouldFail()
    {
        // Arrange
        var sut = new CreateUserGroupCommandValidator();
        var command = new CreateUserGroupCommand
        {
            Name = "A",
            Timezone = "UTC",
            OwnerId = Guid.NewGuid()
        };

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Name" &&
            e.ErrorMessage == "User group name must be at least 2 characters");
    }

    [Fact]
    public void Validate_NameTooLong_ShouldFail()
    {
        // Arrange
        var sut = new CreateUserGroupCommandValidator();
        var command = new CreateUserGroupCommand
        {
            Name = new string('A', 101),
            Timezone = "UTC",
            OwnerId = Guid.NewGuid()
        };

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Name" &&
            e.ErrorMessage == "User group name must not exceed 100 characters");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_TimezoneIsEmpty_ShouldFail(string? timezone)
    {
        // Arrange
        var sut = new CreateUserGroupCommandValidator();
        var command = new CreateUserGroupCommand
        {
            Name = "Valid Group",
            Timezone = timezone!,
            OwnerId = Guid.NewGuid()
        };

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Timezone" &&
            e.ErrorMessage == "Timezone is required");
    }

    [Fact]
    public void Validate_TimezoneTooLong_ShouldFail()
    {
        // Arrange
        var sut = new CreateUserGroupCommandValidator();
        var command = new CreateUserGroupCommand
        {
            Name = "Valid Group",
            Timezone = new string('A', 51),
            OwnerId = Guid.NewGuid()
        };

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Timezone" &&
            e.ErrorMessage == "Timezone must not exceed 50 characters");
    }

    [Fact]
    public void Validate_OwnerIdIsEmpty_ShouldFail()
    {
        // Arrange
        var sut = new CreateUserGroupCommandValidator();
        var command = new CreateUserGroupCommand
        {
            Name = "Valid Group",
            Timezone = "UTC",
            OwnerId = Guid.Empty
        };

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "OwnerId" &&
            e.ErrorMessage == "Owner ID is required");
    }
}