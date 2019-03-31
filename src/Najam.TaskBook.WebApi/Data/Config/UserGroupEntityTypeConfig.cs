using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Najam.TaskBook.WebApi.Data.Entities;

namespace Najam.TaskBook.WebApi.Data.Config
{
    public class UserGroupEntityTypeConfig : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasKey(ug => new {ug.UserId, ug.GroupId});
        }
    }
}
