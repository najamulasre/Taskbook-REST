using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Najam.TaskBook.Domain;

namespace Najam.TaskBook.Data.Configurations
{
    public class UserGroupEntityTypeConfig : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasKey(ug => new {ug.UserId, ug.GroupId});
        }
    }
}
