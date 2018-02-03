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

        Task DeleteGroup(Guid groupId);

        Task<UserGroup> UpdateGroup(Guid userId, Guid groupId, string groupName, bool isActive);
    }
}