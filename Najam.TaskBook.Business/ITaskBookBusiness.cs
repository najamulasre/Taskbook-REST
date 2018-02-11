using System;
using System.Threading.Tasks;
using Najam.TaskBook.Domain;
using Task = Najam.TaskBook.Domain.Task;


namespace Najam.TaskBook.Business
{
    public interface ITaskBookBusiness
    {
        Task<DateTime> GetServerDateTime();

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

        Task<bool> IsUserRelatedWithGroup(Guid userId, Guid groupId);

        Task<Domain.Task[]> GetTasksByGroupId(Guid groupId);

        Task<Domain.Task> GetTaskByTaskId(Guid taskId);

        Task<Domain.Task> CreateGroupTask(Guid groupId, string title, string description, DateTime deadline, Guid createdByUserId);

        Task<bool> IsUserTaskCreator(Guid userId, Guid taskId);

        Task<Task> UpdateGroupTask(Guid taskId, string title, string description, DateTime deadline);

        Task<bool> DeleteTask(Guid taskId);

        Task<Task[]> GetUsersTaskByUserId(Guid userId);

        Task<Task> GetUsersTaskByUserAndTaskId(Guid userId, Guid taskId);
    }
}