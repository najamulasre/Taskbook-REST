using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Najam.TaskBook.WebApi.Data.Entities;

namespace Najam.TaskBook.WebApi.Data.Config
{
    public class UserEntityTypeConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Id)
                .HasDefaultValueSql("newsequentialid()");

            builder.Property(u => u.DateOfBirth)
                .HasColumnType("date");

            builder.Property(u => u.FirstName).HasMaxLength(100);

            builder.Property(u => u.LastName).HasMaxLength(100);
        }
    }
}
