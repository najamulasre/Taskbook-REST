using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Najam.TaskBook.Domain;

namespace Najam.TaskBook.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Id)
                .HasDefaultValueSql("newsequentialid()");

            builder.Property(u => u.DateOfBirth)
                .HasColumnType("date");
        }
    }
}
