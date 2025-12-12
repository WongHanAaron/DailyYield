using DailyYield.Application.Commands;
using DailyYield.Application.Handlers;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using FluentAssertions;
using Moq;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace DailyYield.Application.Tests.Handlers;

public class CreateUserGroupCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateUserGroupAndMember()
    {
        // Arrange
        var userGroupRepositoryMock = new Mock<IRepository<UserGroup>>();
        var memberRepositoryMock = new Mock<IRepository<UserGroupMember>>();
        var sut = new CreateUserGroupCommandHandler(
            userGroupRepositoryMock.Object,
            memberRepositoryMock.Object);

        var command = new CreateUserGroupCommand
        {
            Name = "Test Group",
            Timezone = "UTC",
            OwnerId = Guid.NewGuid()
        };

        var createdUserGroupId = Guid.NewGuid();
        userGroupRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<UserGroup>()))
            .Callback<UserGroup>(ug => ug.Id = createdUserGroupId)
            .Returns(Task.CompletedTask);

        memberRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<UserGroupMember>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(createdUserGroupId);

        userGroupRepositoryMock.Verify(x => x.AddAsync(It.Is<UserGroup>(ug =>
            ug.Name == command.Name &&
            ug.Timezone == command.Timezone)), Times.Once);

        memberRepositoryMock.Verify(x => x.AddAsync(It.Is<UserGroupMember>(member =>
            member.UserId == command.OwnerId &&
            member.UserGroupId == createdUserGroupId &&
            member.Role == "owner")), Times.Once);
    }

    [Fact]
    public async Task Handle_UserGroupRepositoryThrows_ShouldPropagateException()
    {
        // Arrange
        var userGroupRepositoryMock = new Mock<IRepository<UserGroup>>();
        var memberRepositoryMock = new Mock<IRepository<UserGroupMember>>();
        var sut = new CreateUserGroupCommandHandler(
            userGroupRepositoryMock.Object,
            memberRepositoryMock.Object);

        var command = new CreateUserGroupCommand
        {
            Name = "Test Group",
            Timezone = "UTC",
            OwnerId = Guid.NewGuid()
        };

        var expectedException = new Exception("Database error");
        userGroupRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<UserGroup>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            sut.Handle(command, CancellationToken.None));

        exception.Should().Be(expectedException);
    }

    [Fact]
    public async Task Handle_MemberRepositoryThrows_ShouldPropagateException()
    {
        // Arrange
        var userGroupRepositoryMock = new Mock<IRepository<UserGroup>>();
        var memberRepositoryMock = new Mock<IRepository<UserGroupMember>>();
        var sut = new CreateUserGroupCommandHandler(
            userGroupRepositoryMock.Object,
            memberRepositoryMock.Object);

        var command = new CreateUserGroupCommand
        {
            Name = "Test Group",
            Timezone = "UTC",
            OwnerId = Guid.NewGuid()
        };

        var createdUserGroupId = Guid.NewGuid();
        userGroupRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<UserGroup>()))
            .Callback<UserGroup>(ug => ug.Id = createdUserGroupId)
            .Returns(Task.CompletedTask);

        var expectedException = new Exception("Database error");
        memberRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<UserGroupMember>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            sut.Handle(command, CancellationToken.None));

        exception.Should().Be(expectedException);
    }
}