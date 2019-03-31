using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Najam.TaskBook.WebApi.Data.Entities;

namespace Najam.TaskBook.WebApi.Data.Config
{
    public class GroupEntityTypeConfig : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.DateCreated)
                .HasColumnType("date")
                .HasDefaultValueSql("getdate()")
                .ValueGeneratedOnAdd();
        }
    }
}
