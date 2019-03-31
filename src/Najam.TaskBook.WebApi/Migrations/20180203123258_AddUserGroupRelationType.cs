using Microsoft.EntityFrameworkCore.Migrations;

namespace Najam.TaskBook.WebApi.Migrations
{
    public partial class AddUserGroupRelationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelationType",
                table: "UserGroups",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelationType",
                table: "UserGroups");
        }
    }
}
