using DailyYield.Application.Handlers;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using FluentAssertions;
using Moq;
using Xunit;
using AutoMapper;
using Task = System.Threading.Tasks.Task;

namespace DailyYield.Application.Tests.Handlers;

public class GetUserGroupsQueryHandlerTests
{
    public async Task Handle_UserHasMemberships_ShouldReturnUserGroupDtos()
    {
        // Arrange
        var memberRepositoryMock = new Mock<IRepository<UserGroupMember>>();
        var mapperMock = new Mock<IMapper>();
        var sut = new GetUserGroupsQueryHandler(memberRepositoryMock.Object, mapperMock.Object);

        var userId = Guid.NewGuid();
        var userGroup1 = new UserGroup { Id = Guid.NewGuid(), Name = "Group 1", Timezone = "UTC" };
        var userGroup2 = new UserGroup { Id = Guid.NewGuid(), Name = "Group 2", Timezone = "EST" };

        var memberships = new List<UserGroupMember>
        {
            new UserGroupMember { UserId = userId, UserGroup = userGroup1, Role = "owner" },
            new UserGroupMember { UserId = userId, UserGroup = userGroup2, Role = "member" },
            new UserGroupMember { UserId = Guid.NewGuid(), UserGroup = userGroup1, Role = "member" } // Different user
        };

        memberRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(memberships);

        var expectedDtos = new List<UserGroupDto>
        {
            new UserGroupDto { Id = userGroup1.Id, Name = "Group 1", Timezone = "UTC", Role = "owner" },
            new UserGroupDto { Id = userGroup2.Id, Name = "Group 2", Timezone = "EST", Role = "member" }
        };

        mapperMock
            .Setup(x => x.Map<IEnumerable<UserGroupDto>>(It.IsAny<IEnumerable<UserGroupMember>>()))
            .Returns(expectedDtos);

        var query = new GetUserGroupsQuery { UserId = userId };

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        var resultList = result.ToList();
        resultList[0].Id.Should().Be(userGroup1.Id);
        resultList[0].Name.Should().Be("Group 1");
        resultList[0].Timezone.Should().Be("UTC");
        resultList[0].Role.Should().Be("owner");

        resultList[1].Id.Should().Be(userGroup2.Id);
        resultList[1].Name.Should().Be("Group 2");
        resultList[1].Timezone.Should().Be("EST");
        resultList[1].Role.Should().Be("member");
    }

    [Fact]
    public async Task Handle_UserHasNoMemberships_ShouldReturnEmptyList()
    {
        // Arrange
        var memberRepositoryMock = new Mock<IRepository<UserGroupMember>>();
        var mapperMock = new Mock<IMapper>();
        var sut = new GetUserGroupsQueryHandler(memberRepositoryMock.Object, mapperMock.Object);

        var userId = Guid.NewGuid();
        var memberships = new List<UserGroupMember>
        {
            new UserGroupMember { UserId = Guid.NewGuid(), UserGroup = new UserGroup(), Role = "member" }
        };

        memberRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(memberships);

        var query = new GetUserGroupsQuery { UserId = userId };

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_NoMembershipsExist_ShouldReturnEmptyList()
    {
        // Arrange
        var memberRepositoryMock = new Mock<IRepository<UserGroupMember>>();
        var mapperMock = new Mock<IMapper>();
        var sut = new GetUserGroupsQueryHandler(memberRepositoryMock.Object, mapperMock.Object);

        var userId = Guid.NewGuid();
        var memberships = new List<UserGroupMember>();

        memberRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(memberships);

        var query = new GetUserGroupsQuery { UserId = userId };

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}