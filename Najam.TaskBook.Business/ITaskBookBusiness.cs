using System;
using System.Threading.Tasks;
using Najam.TaskBook.Domain;


namespace Najam.TaskBook.Business
{
    public interface ITaskBookBusiness
    {
        Task<UserGroup[]> GetUserGroupsByUserId(Guid userId);

        Task<UserGroup> GetUserGroupByGroupId(Guid userId, Guid groupId);

        Task<UserGroup> CreateUserGroup(Guid userId, string groupName, bool isActive);

        System.Threading.Tasks.Task DeleteGroup(Guid groupId);

        Task<UserGroup> UpdateGroup(Guid userId, Guid groupId, string groupName, bool isActive);

        Task<bool> IsUserGroupOwner(Guid userId, Guid groupId);

        Task<UserGroup[]> GetGroupMemberships(Guid groupId);

        Task<UserGroup> GetGroupMembership(Guid userId, Guid groupId);

        Task<UserGroup> CrateGroupMembership(Guid userId, Guid groupId);

        System.Threading.Tasks.Task DeleteGroupMembership(Guid userId, Guid groupId);

        Task<UserGroup[]> GetUserMemberships(Guid userId);
    }
}