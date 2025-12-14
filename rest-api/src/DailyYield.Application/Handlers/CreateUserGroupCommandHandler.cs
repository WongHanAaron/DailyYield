using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using Serilog;

namespace DailyYield.Application.Handlers;

public class CreateUserGroupCommandHandler : IRequestHandler<CreateUserGroupCommand, Guid>
{
    private readonly IRepository<UserGroup> _userGroupRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;

    public CreateUserGroupCommandHandler(
        IRepository<UserGroup> userGroupRepository,
        IRepository<UserGroupMember> memberRepository)
    {
        _userGroupRepository = userGroupRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Guid> Handle(CreateUserGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Information("Creating user group with name: {Name} for owner: {OwnerId}", request.Name, request.OwnerId);

            var userGroup = new UserGroup
            {
                Name = request.Name,
                Timezone = request.Timezone
            };

            await _userGroupRepository.AddAsync(userGroup);

            var member = new UserGroupMember
            {
                UserId = request.OwnerId,
                UserGroupId = userGroup.Id,
                Role = "owner"
            };

            await _memberRepository.AddAsync(member);

            Log.Information("Successfully created user group {UserGroupId} with owner {OwnerId}", userGroup.Id, request.OwnerId);

            return userGroup.Id;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to create user group with name: {Name} for owner: {OwnerId}", request.Name, request.OwnerId);
            throw;
        }
    }
}