using DailyYield.Application.Commands;
using DailyYield.Application.Handlers;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using FluentAssertions;
using Moq;
using TaskEntity = DailyYield.Domain.Entities.Task;
using Task = System.Threading.Tasks.Task;
using Xunit;

namespace DailyYield.Application.Tests.Handlers;

public class CreateTaskCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateTaskAndReturnId()
    {
        // Arrange
        var taskRepositoryMock = new Mock<IRepository<TaskEntity>>();
        var sut = new CreateTaskCommandHandler(taskRepositoryMock.Object);

        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            UserId = Guid.NewGuid(),
            UserGroupId = Guid.NewGuid(),
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        var createdTaskId = Guid.NewGuid();
        taskRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<TaskEntity>()))
            .Callback<TaskEntity>(t => t.Id = createdTaskId)
            .Returns(Task.CompletedTask);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(createdTaskId);

        taskRepositoryMock.Verify(x => x.AddAsync(It.Is<TaskEntity>(t =>
            t.Title == command.Title &&
            t.Description == command.Description &&
            t.UserId == command.UserId &&
            t.UserGroupId == command.UserGroupId &&
            t.DueDate == command.DueDate)), Times.Once);
    }

    [Fact]
    public async Task Handle_MinimalCommand_ShouldCreateTaskWithDefaults()
    {
        // Arrange
        var taskRepositoryMock = new Mock<IRepository<TaskEntity>>();
        var sut = new CreateTaskCommandHandler(taskRepositoryMock.Object);

        var command = new CreateTaskCommand
        {
            Title = "Minimal Task",
            UserId = Guid.NewGuid()
        };

        var createdTaskId = Guid.NewGuid();
        taskRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<TaskEntity>()))
            .Callback<TaskEntity>(t => t.Id = createdTaskId)
            .Returns(Task.CompletedTask);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(createdTaskId);

        taskRepositoryMock.Verify(x => x.AddAsync(It.Is<TaskEntity>(t =>
            t.Title == command.Title &&
            t.Description == command.Description &&
            t.UserId == command.UserId &&
            t.UserGroupId == command.UserGroupId &&
            t.DueDate == command.DueDate)), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrows_ShouldPropagateException()
    {
        // Arrange
        var taskRepositoryMock = new Mock<IRepository<TaskEntity>>();
        var sut = new CreateTaskCommandHandler(taskRepositoryMock.Object);

        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            UserId = Guid.NewGuid()
        };

        var expectedException = new Exception("Database error");
        taskRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<TaskEntity>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            sut.Handle(command, CancellationToken.None));

        exception.Should().Be(expectedException);
    }
}