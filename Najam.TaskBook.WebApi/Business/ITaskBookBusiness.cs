using System;
using System.Threading.Tasks;
using Najam.TaskBook.WebApi.Business.Dtos;
using Najam.TaskBook.WebApi.Data.Entities;
using Task = Najam.TaskBook.WebApi.Data.Entities.Task;


namespace Najam.TaskBook.WebApi.Business
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

        Task<Data.Entities.Task[]> GetTasksByGroupId(Guid groupId);

        Task<Data.Entities.Task> GetTaskByTaskId(Guid taskId);

        Task<Task> CreateGroupTask(Guid groupId, string title, string description, DateTime deadline, Guid createdByUserId);

        Task<bool> IsUserTaskCreator(Guid userId, Guid taskId);

        Task<Task> UpdateGroupTask(Guid taskId, string title, string description, DateTime deadline);

        Task<bool> DeleteTask(Guid taskId);

        Task<UserTaskPage> GetUsersTaskByUserId(Guid userId, GetUserTasksParameters parameters);

        Task<Task> GetUsersTaskByUserAndTaskId(Guid userId, Guid taskId);

        Task<Task[]> GetUsersTaskAssignmentsByUserId(Guid userId);

        Task<Task> GetUsersTaskAssignmentByUserAndTaskId(Guid userId, Guid taskId);

        Task<Task> CreateTaskAssignmen(Guid assignToUserId, Guid taskId);

        Task<bool> DeleteTaskAssignment(Guid taskId);

        Task<Task[]> GetUsersTaskCompletionsByUserId(Guid userId);

        Task<Task> GetUsersTaskCompletionByUserAndTaskId(Guid userId, Guid taskId);

        Task<Task> CreateTaskCompletion(Guid assignToUserId, Guid taskId);

        Task<bool> DeleteTaskCompletion(Guid taskId);
    }
}