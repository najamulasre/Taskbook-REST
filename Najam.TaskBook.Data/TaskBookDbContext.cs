using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Najam.TaskBook.Data.Configurations;
using Najam.TaskBook.Domain;

namespace Najam.TaskBook.Data
{
    public class TaskBookDbContext : IdentityDbContext<User, Role, Guid>
    {
        public TaskBookDbContext(DbContextOptions<TaskBookDbContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new UserEntityTypeConfig())
                .ApplyConfiguration(new RoleEntityTypeConfig());

            base.OnModelCreating(builder);
        }
    }
}
