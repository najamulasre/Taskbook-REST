using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Najam.TaskBook.Business.Dtos;
using Najam.TaskBook.Data;
using Najam.TaskBook.Domain;
using Task = Najam.TaskBook.Domain.Task;


namespace Najam.TaskBook.Business
{
    public class TaskBookBusiness : ITaskBookBusiness
    {
        private readonly TaskBookDbContext _dbContext;

        public TaskBookBusiness(TaskBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DateTime> GetServerDateTime()
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select getdate()";
                _dbContext.Database.OpenConnection();
                object result = await command.ExecuteScalarAsync();

                return (DateTime)result;
            }
        }

        public Task<UserGroup[]> GetUserGroupsByUserId(Guid userId)
        {
            IQueryable<UserGroup> query = _dbContext.UserGroups
                .Include(ug => ug.Group)
                .Where(ug => ug.UserId == userId && ug.RelationType == UserGroupRelationType.Owner)
                .OrderBy(ug => ug.Group.Name);

            return query.ToArrayAsync();
        }

        public Task<UserGroup> GetUserGroupByGroupId(Guid userId, Guid groupId)
        {
            IQueryable<UserGroup> query = _dbContext.UserGroups
                .Include(ug => ug.Group)
                .Where(ug => ug.UserId == userId && ug.GroupId == groupId && ug.RelationType == UserGroupRelationType.Owner);

            return query.SingleOrDefaultAsync();
        }

        public async Task<UserGroup> CreateUserGroup(Guid userId, string groupName, bool isActive)
        {
            // add group
            var group = new Group
            {
                Id = Guid.NewGuid(),
                Name = groupName,
                IsActive = isActive
            };

            // add relationship with user
            var userGroup = new UserGroup
            {
                UserId = userId,
                Group = group,
                RelationType = UserGroupRelationType.Owner
            };

            _dbContext.UserGroups.Add(userGroup);
            _dbContext.Groups.Add(group);

            await _dbContext.SaveChangesAsync();

            return await GetUserGroupByGroupId(userId, group.Id);
        }

        public System.Threading.Tasks.Task DeleteGroup(Guid groupId)
        {
            Group group = _dbContext.Groups.Single(g => g.Id == groupId);
            _dbContext.Groups.Remove(group);

            return _dbContext.SaveChangesAsync();
        }

        public async Task<UserGroup> UpdateGroup(Guid userId, Guid groupId, string groupName, bool isActive)
        {
            Group group = _dbContext.Groups.Single(g => g.Id == groupId);

            group.Name = groupName;
            group.IsActive = isActive;

            await _dbContext.SaveChangesAsync();

            return await GetUserGroupByGroupId(userId, group.Id);
        }

        public Task<bool> IsUserGroupOwner(Guid userId, Guid groupId)
        {
            IQueryable<UserGroup> query = _dbContext.UserGroups
                .Where(ug => ug.UserId == userId && ug.GroupId == groupId && ug.RelationType == UserGroupRelationType.Owner);

            return query.AnyAsync();
        }

        public Task<UserGroup[]> GetGroupMemberships(Guid groupId)
        {
            IQueryable<UserGroup> query = _dbContext.UserGroups
                .Include(ug => ug.Group)
                .Include(ug => ug.User)
                .Where(ug => ug.GroupId == groupId && ug.RelationType == UserGroupRelationType.Member)
                .OrderBy(ug => ug.User.UserName);

            return query.ToArrayAsync();
        }

        public Task<UserGroup> GetGroupMembership(Guid userId, Guid groupId)
        {
            return _dbContext.UserGroups
                .Include(ug => ug.Group)
                .Include(ug => ug.User)
                .SingleOrDefaultAsync(ug => ug.GroupId == groupId && ug.UserId == userId);
        }

        public async Task<UserGroup> CrateGroupMembership(Guid userId, Guid groupId)
        {
            var userGroup = new UserGroup
            {
                UserId = userId,
                GroupId = groupId,
                RelationType = UserGroupRelationType.Member
            };

            _dbContext.UserGroups.Add(userGroup);
            await _dbContext.SaveChangesAsync();

            return await GetGroupMembership(userId, groupId);
        }

        public async System.Threading.Tasks.Task DeleteGroupMembership(Guid userId, Guid groupId)
        {
            UserGroup group = await GetGroupMembership(userId, groupId);

            if (group == null)
                return;

            _dbContext.UserGroups.Remove(group);
            await _dbContext.SaveChangesAsync();
        }

        public Task<UserGroup[]> GetUserMemberships(Guid userId)
        {
            IQueryable<UserGroup> query = _dbContext.UserGroups
                .Include(ug => ug.User)
                .Include(ug => ug.Group)
                .Where(ug => ug.UserId == userId)
                .OrderBy(ug => ug.Group.Name);

            return query.ToArrayAsync();
        }

        public Task<bool> IsUserRelatedWithGroup(Guid userId, Guid groupId)
        {
            return _dbContext.UserGroups
                .AnyAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
        }

        public Task<Task[]> GetTasksByGroupId(Guid groupId)
        {
            IQueryable<Task> query = _dbContext.Tasks
                .Include(t => t.Group)
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .Where(t => t.GroupId == groupId)
                .OrderBy(t => t.Deadline);

            return query.ToArrayAsync();
        }

        public Task<Task> GetTaskByTaskId(Guid taskId)
        {
            IQueryable<Task> query = _dbContext.Tasks
                .Include(t => t.Group)
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .Where(t => t.Id == taskId);

            return query.SingleOrDefaultAsync();
        }

        public async Task<Task> CreateGroupTask(Guid groupId, string title, string description, DateTime deadline, Guid createdByUserId)
        {
            var task = new Task
            {
                Id = Guid.NewGuid(),
                GroupId = groupId,
                Title = title,
                Description = description,
                Deadline = deadline,
                CreatedByUserId = createdByUserId
            };

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            return await GetTaskByTaskId(task.Id);
        }

        public Task<bool> IsUserTaskCreator(Guid userId, Guid taskId)
        {
            return _dbContext.Tasks.AnyAsync(t => t.Id == taskId && t.CreatedByUserId == userId);
        }

        public async Task<Task> UpdateGroupTask(Guid taskId, string title, string description, DateTime deadline)
        {
            Task task = await _dbContext.Tasks.FindAsync(taskId);

            if (task == null)
                return null;

            task.Title = title;
            task.Description = description;
            task.Deadline = deadline;

            await _dbContext.SaveChangesAsync();

            return task;
        }

        public async Task<bool> DeleteTask(Guid taskId)
        {
            Task task = await _dbContext.Tasks.FindAsync(taskId);

            if (task == null)
                return false;

            _dbContext.Tasks.Remove(task);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<UserTaskPage> GetUsersTaskByUserId(Guid userId, GetUserTasksParameters parameters)
        {
            int skip = (parameters.PageNumber - 1) * parameters.PageSize;
            int take = parameters.PageSize;

            IQueryable<Task> query = _dbContext.UserGroups
                .Where(g => g.UserId == userId)
                .SelectMany(g => g.Group.Tasks)
                .Include(t => t.Group)
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser);

            // Search Query
            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
                query = query.Where(t => t.Title.Contains(parameters.SearchQuery) || t.Description.Contains(parameters.SearchQuery));

            // Filtering
            if (!string.IsNullOrWhiteSpace(parameters.GroupName))
                query = query.Where(t => t.Group.Name == parameters.GroupName);

            if (parameters.Overdue.HasValue)
                query = query.Where(t => t.IsOverdue == parameters.Overdue);

            if (!string.IsNullOrEmpty(parameters.CreatedBy))
                query = query.Where(t => t.CreatedByUser.UserName == parameters.CreatedBy);

            if (!string.IsNullOrWhiteSpace(parameters.AssignedTo))
                query = query.Where(t => t.AssignedToUserId.HasValue && t.AssignedToUser.UserName == parameters.AssignedTo);

            // Sorting
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                string orderBy = parameters.OrderBy
                    .Replace(" ", string.Empty)
                    .ToLower();

                switch (orderBy)
                {
                    case "groupname":
                        query = query.OrderBy(t => t.Group.Name);
                        break;
                    case "-groupname":
                        query = query.OrderByDescending(t => t.Group.Name);
                        break;

                    case "title":
                        query = query.OrderBy(t => t.Title);
                        break;
                    case "-title":
                        query = query.OrderByDescending(t => t.Title);
                        break;

                    case "datetimecreated":
                        query = query.OrderBy(t => t.DateTimeCreated);
                        break;
                    case "-datetimecreated":
                        query = query.OrderByDescending(t => t.DateTimeCreated);
                        break;

                    case "deadline":
                        query = query.OrderBy(t => t.Deadline);
                        break;
                    case "-deadline":
                        query = query.OrderByDescending(t => t.Deadline);
                        break;

                    case "datetimecompleted":
                        query = query.OrderBy(t => t.DateTimeCompleted);
                        break;
                    case "-datetimecompleted":
                        query = query.OrderByDescending(t => t.DateTimeCompleted);
                        break;

                    case "isoverdue":
                        query = query.OrderBy(t => t.IsOverdue);
                        break;
                    case "-isoverdue":
                        query = query.OrderByDescending(t => t.IsOverdue);
                        break;

                    case "createdby":
                        query = query.OrderBy(t => t.CreatedByUser.UserName);
                        break;
                    case "-createdby":
                        query = query.OrderByDescending(t => t.CreatedByUser.UserName);
                        break;

                    case "assignedto":
                        query = query.OrderBy(t => t.AssignedToUser.UserName);
                        break;
                    case "-assignedto":
                        query = query.OrderByDescending(t => t.AssignedToUser.UserName);
                        break;

                    case "datetimeassigned":
                        query = query.OrderBy(t => t.DateTimeAssigned);
                        break;
                    case "-datetimeassigned":
                        query = query.OrderByDescending(t => t.DateTimeAssigned);
                        break;

                    default:
                        query = query.OrderBy(t => t.Deadline);
                        break;
                }
            }

            // Paging
            int totalCount = query.Count();

            query = query
                .Skip(skip)
                .Take(take);

            Task[] tasks = await query.ToArrayAsync();

            var page = new UserTaskPage(parameters.PageNumber, parameters.PageSize, totalCount, tasks);

            return page;
        }

        public Task<Task> GetUsersTaskByUserAndTaskId(Guid userId, Guid taskId)
        {
            IQueryable<Task> query = _dbContext.UserGroups
                .Where(g => g.UserId == userId)
                .SelectMany(g => g.Group.Tasks)
                .Where(t => t.Id == taskId)
                .Include(t => t.Group)
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser);

            return query.SingleOrDefaultAsync();
        }

        public Task<Task[]> GetUsersTaskAssignmentsByUserId(Guid userId)
        {
            IQueryable<Task> query = _dbContext.UserGroups
                .Where(g => g.UserId == userId)
                .SelectMany(g => g.Group.Tasks)
                .Include(t => t.Group)
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .Where(t => t.AssignedToUserId.HasValue && !t.DateTimeCompleted.HasValue)
                .OrderBy(t => t.Deadline);

            return query.ToArrayAsync();
        }

        public Task<Task> GetUsersTaskAssignmentByUserAndTaskId(Guid userId, Guid taskId)
        {
            IQueryable<Task> query = _dbContext.UserGroups
                .Where(g => g.UserId == userId)
                .SelectMany(g => g.Group.Tasks)
                .Where(t => t.Id == taskId)
                .Include(t => t.Group)
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .Where(t => t.AssignedToUserId.HasValue && !t.DateTimeCompleted.HasValue);

            return query.SingleOrDefaultAsync();
        }

        public async Task<Task> CreateTaskAssignmen(Guid assignToUserId, Guid taskId)
        {
            Task task = _dbContext.Tasks.Find(taskId);
            task.AssignedToUserId = assignToUserId;
            task.DateTimeAssigned = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return await GetUsersTaskAssignmentByUserAndTaskId(assignToUserId, taskId);
        }

        public async Task<bool> DeleteTaskAssignment(Guid taskId)
        {
            Task task = _dbContext.Tasks.Find(taskId);

            if (task == null)
                return false;

            task.AssignedToUserId = null;
            task.DateTimeAssigned = null;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public Task<Task[]> GetUsersTaskCompletionsByUserId(Guid userId)
        {
            IQueryable<Task> query = _dbContext.UserGroups
                .Where(g => g.UserId == userId)
                .SelectMany(g => g.Group.Tasks)
                .Include(t => t.Group)
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .Where(t => t.DateTimeCompleted.HasValue)
                .OrderByDescending(t => t.DateTimeCompleted);

            return query.ToArrayAsync();
        }

        public Task<Task> GetUsersTaskCompletionByUserAndTaskId(Guid userId, Guid taskId)
        {
            IQueryable<Task> query = _dbContext.UserGroups
                .Where(g => g.UserId == userId)
                .SelectMany(g => g.Group.Tasks)
                .Where(t => t.Id == taskId)
                .Include(t => t.Group)
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .Where(t => t.DateTimeCompleted.HasValue);

            return query.SingleOrDefaultAsync();
        }

        public async Task<Task> CreateTaskCompletion(Guid assignToUserId, Guid taskId)
        {
            Task task = _dbContext.Tasks.Find(taskId);
            task.DateTimeCompleted = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return await GetUsersTaskCompletionByUserAndTaskId(assignToUserId, taskId);
        }

        public async Task<bool> DeleteTaskCompletion(Guid taskId)
        {
            Task task = _dbContext.Tasks.Find(taskId);

            if (task == null)
                return false;

            task.DateTimeCompleted = null;

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}