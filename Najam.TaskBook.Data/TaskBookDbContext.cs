using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Najam.TaskBook.Data.Configurations;
using Najam.TaskBook.Domain;

namespace Najam.TaskBook.Data
{
    public class TaskBookDbContext : IdentityDbContext<User, Role, Guid>
    {
        /*  1. Create Entity Classes
            2. Define relationships using navigation properties
            3. Create entity type configurations (if needed)
            4. Register configuration in this class's OnModelCreating method (once per type)
            5. Add migrations using add-migration command in Package Manager Console - if there's no error it should create migration code
            6. Run migration code using update-database command in the Package Manager Console
            If there's no error, database tabel should be created
         */

        public DbSet<Group> Groups { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }


        public TaskBookDbContext(DbContextOptions<TaskBookDbContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new UserEntityTypeConfig())
                .ApplyConfiguration(new RoleEntityTypeConfig())
                .ApplyConfiguration(new GroupEntityTypeConfiguration())
                .ApplyConfiguration(new UserGroupEntityTypeConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
