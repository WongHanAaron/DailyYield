using DailyYield.Api.Controllers;
using DailyYield.Application.Commands.AuthenticateUser;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace DailyYield.Api.Tests.Controllers;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sut = new AuthController(mediatorMock.Object);
        var command = new AuthenticateUserCommand
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var authResponse = new AuthenticateUserResponse
        {
            Token = "jwt-token-here",
            UserId = Guid.NewGuid(),
            Email = "test@example.com"
        };

        mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(authResponse);

        // Act
        var result = await sut.Login(command);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeAssignableTo<AuthenticateUserResponse>().Subject;
        response.Token.Should().Be("jwt-token-here");
        response.Email.Should().Be("test@example.com");
        response.UserId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sut = new AuthController(mediatorMock.Object);
        var command = new AuthenticateUserCommand
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        };

        mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

        // Act
        var result = await sut.Login(command);

        // Assert
        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task Login_ValidationError_ReturnsBadRequest()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sut = new AuthController(mediatorMock.Object);
        var command = new AuthenticateUserCommand
        {
            Email = "", // Invalid: empty email
            Password = "password123"
        };

        mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Email is required"));

        // Act
        var result = await sut.Login(command);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Login_WhenExceptionOccurs_ReturnsInternalServerError()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sut = new AuthController(mediatorMock.Object);
        var command = new AuthenticateUserCommand
        {
            Email = "test@example.com",
            Password = "password123"
        };

        mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await sut.Login(command);

        // Assert
        var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
        statusCodeResult.StatusCode.Should().Be(500);
    }
}