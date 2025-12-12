using DailyYield.Api.Controllers;
using DailyYield.Application.Commands.CreateUserGroup;
using DailyYield.Application.Queries.GetUserGroups;
using DailyYield.Domain.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace DailyYield.Api.Tests.Controllers;

public class UserGroupsControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsOkResult_WithUserGroups()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sut = new UserGroupsController(mediatorMock.Object);
        var userGroups = new List<UserGroup>
        {
            new UserGroup { Id = Guid.NewGuid(), Name = "Group 1", Timezone = "UTC" },
            new UserGroup { Id = Guid.NewGuid(), Name = "Group 2", Timezone = "EST" }
        };

        mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserGroupsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userGroups);

        // Act
        var result = await sut.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedGroups = okResult.Value.Should().BeAssignableTo<IEnumerable<UserGroup>>().Subject;
        returnedGroups.Should().HaveCount(2);
        returnedGroups.Should().Contain(ug => ug.Name == "Group 1");
        returnedGroups.Should().Contain(ug => ug.Name == "Group 2");
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithEmptyList_WhenNoUserGroups()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sut = new UserGroupsController(mediatorMock.Object);
        var userGroups = new List<UserGroup>();

        mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserGroupsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userGroups);

        // Act
        var result = await sut.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedGroups = okResult.Value.Should().BeAssignableTo<IEnumerable<UserGroup>>().Subject;
        returnedGroups.Should().BeEmpty();
    }

    [Fact]
    public async Task Create_ValidCommand_ReturnsCreatedResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sut = new UserGroupsController(mediatorMock.Object);
        var command = new CreateUserGroupCommand
        {
            Name = "New Group",
            Timezone = "UTC"
        };

        var createdGroup = new UserGroup
        {
            Id = Guid.NewGuid(),
            Name = "New Group",
            Timezone = "UTC"
        };

        mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdGroup);

        // Act
        var result = await sut.Create(command);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(UserGroupsController.GetAll));
        var returnedGroup = createdResult.Value.Should().BeAssignableTo<UserGroup>().Subject;
        returnedGroup.Name.Should().Be("New Group");
        returnedGroup.Timezone.Should().Be("UTC");
    }

    [Fact]
    public async Task Create_InvalidCommand_ReturnsBadRequest()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sut = new UserGroupsController(mediatorMock.Object);
        var command = new CreateUserGroupCommand
        {
            Name = "", // Invalid: empty name
            Timezone = "UTC"
        };

        mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Name is required"));

        // Act
        var result = await sut.Create(command);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_WhenExceptionOccurs_ReturnsInternalServerError()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sut = new UserGroupsController(mediatorMock.Object);
        var command = new CreateUserGroupCommand
        {
            Name = "Test Group",
            Timezone = "UTC"
        };

        mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await sut.Create(command);

        // Assert
        var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
        statusCodeResult.StatusCode.Should().Be(500);
    }
}