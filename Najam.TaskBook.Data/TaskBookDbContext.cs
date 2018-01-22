using System;
using Microsoft.AspNetCore.Identity;
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
            builder.Entity<IdentityUserLogin<Guid>>()
                .HasKey(l => new {l.LoginProvider, l.ProviderKey});

            builder
                .ApplyConfiguration(new UserConfiguration())
                .ApplyConfiguration(new RoleConfiguration());

        }
    }
}
