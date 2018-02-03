using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Najam.TaskBook.Data;
using Najam.TaskBook.Domain;

namespace Najam.TaskBook.Business
{
    public class TaskBookBusiness : ITaskBookBusiness
    {
        private readonly TaskBookDbContext _dbContext;

        public TaskBookBusiness(TaskBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<UserGroup[]> GetUserGroupsByUserId(Guid userId)
        {
            IQueryable<UserGroup> query = _dbContext.UserGroups
                .Include(ug => ug.Group)
                .Where(ug => ug.UserId == userId);

            return query.ToArrayAsync();
        }

        public Task<UserGroup> GetUserGroupByGroupId(Guid userId, Guid groupId)
        {
            IQueryable<UserGroup> query = _dbContext.UserGroups
                .Include(ug => ug.Group)
                .Where(ug => ug.UserId == userId && ug.GroupId == groupId);

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

        public Task DeleteGroup(Guid groupId)
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
    }
}