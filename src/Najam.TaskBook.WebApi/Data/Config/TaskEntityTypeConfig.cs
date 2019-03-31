using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Najam.TaskBook.WebApi.Data.Entities;

namespace Najam.TaskBook.WebApi.Data.Config
{
    public class TaskEntityTypeConfig : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(g => g.DateTimeCreated)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("getdate()")
                .ValueGeneratedOnAdd();

            builder
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.TasksCreated)
                .IsRequired()
                .HasForeignKey(t => t.CreatedByUserId)
                .HasConstraintName("FK_Task_AspNetUsers_CreatedByUserId");

            builder
                .HasOne(t => t.AssignedToUser)
                .WithMany(u => u.TasksAssigned)
                .IsRequired(false)
                .HasForeignKey(t => t.AssignedToUserId)
                .HasConstraintName("FK_Task_AspNetUsersAssignedToUserId");

            builder.Property(t => t.IsOverdue)
                .HasComputedColumnSql("case when DateTimeCompleted is null and Deadline < getdate() then cast(1 as bit) else cast(0 as bit) end");
        }
    }
}
