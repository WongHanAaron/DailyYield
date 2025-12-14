using DailyYield.Adapter.Database;
using DailyYield.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace DailyYield.Adapter.Database.Tests;

public class EFRepositoryTests : IDisposable
{
    [Fact]
    public async Task AddAsync_ValidEntity_ShouldAddToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DailyYieldDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new DailyYieldDbContext(options);
        var sut = new EFRepository<UserGroup>(context);
        var userGroup = new UserGroup
        {
            Name = "Test Group",
            Timezone = "UTC"
        };

        // Act
        await sut.AddAsync(userGroup);

        // Assert
        var savedEntity = await context.UserGroups.FindAsync(userGroup.Id);
        savedEntity.Should().NotBeNull();
        savedEntity!.Name.Should().Be("Test Group");
        savedEntity.Timezone.Should().Be("UTC");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingEntity_ShouldReturnEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DailyYieldDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new DailyYieldDbContext(options);
        var sut = new EFRepository<UserGroup>(context);
        var userGroup = new UserGroup
        {
            Name = "Test Group",
            Timezone = "UTC"
        };
        await context.UserGroups.AddAsync(userGroup);
        await context.SaveChangesAsync();

        // Act
        var result = await sut.GetByIdAsync(userGroup.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Group");
        result.Timezone.Should().Be("UTC");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingEntity_ShouldReturnNull()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DailyYieldDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new DailyYieldDbContext(options);
        var sut = new EFRepository<UserGroup>(context);

        // Act
        var result = await sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_HasEntities_ShouldReturnAllEntities()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DailyYieldDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new DailyYieldDbContext(options);
        var sut = new EFRepository<UserGroup>(context);
        var userGroup1 = new UserGroup { Name = "Group 1", Timezone = "UTC" };
        var userGroup2 = new UserGroup { Name = "Group 2", Timezone = "EST" };
        await context.UserGroups.AddRangeAsync(userGroup1, userGroup2);
        await context.SaveChangesAsync();

        // Act
        var result = await sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(ug => ug.Name == "Group 1");
        result.Should().Contain(ug => ug.Name == "Group 2");
    }

    [Fact]
    public async Task GetAllAsync_NoEntities_ShouldReturnEmptyList()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DailyYieldDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new DailyYieldDbContext(options);
        var sut = new EFRepository<UserGroup>(context);

        // Act
        var result = await sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_ExistingEntity_ShouldUpdateInDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DailyYieldDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new DailyYieldDbContext(options);
        var sut = new EFRepository<UserGroup>(context);
        var userGroup = new UserGroup
        {
            Name = "Original Name",
            Timezone = "UTC"
        };
        await context.UserGroups.AddAsync(userGroup);
        await context.SaveChangesAsync();

        // Act
        userGroup.Name = "Updated Name";
        await sut.UpdateAsync(userGroup);

        // Assert
        var updatedEntity = await context.UserGroups.FindAsync(userGroup.Id);
        updatedEntity.Should().NotBeNull();
        updatedEntity!.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task DeleteAsync_ExistingEntity_ShouldRemoveFromDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DailyYieldDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new DailyYieldDbContext(options);
        var sut = new EFRepository<UserGroup>(context);
        var userGroup = new UserGroup
        {
            Name = "Test Group",
            Timezone = "UTC"
        };
        await context.UserGroups.AddAsync(userGroup);
        await context.SaveChangesAsync();

        // Act
        await sut.DeleteAsync(userGroup);

        // Assert
        var deletedEntity = await context.UserGroups.FindAsync(userGroup.Id);
        deletedEntity.Should().BeNull();
    }

    public void Dispose()
    {
        // Cleanup is handled per test method now
    }
}