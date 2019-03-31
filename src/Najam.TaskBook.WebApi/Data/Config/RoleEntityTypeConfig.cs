using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Najam.TaskBook.WebApi.Data.Entities;

namespace Najam.TaskBook.WebApi.Data.Config
{
    public class RoleEntityTypeConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(u => u.Id)
                .HasDefaultValueSql("newsequentialid()");
        }
    }
}
