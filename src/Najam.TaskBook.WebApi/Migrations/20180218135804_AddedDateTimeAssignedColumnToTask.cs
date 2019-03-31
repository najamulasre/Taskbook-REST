using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Najam.TaskBook.WebApi.Migrations
{
    public partial class AddedDateTimeAssignedColumnToTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeAssigned",
                table: "Tasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeAssigned",
                table: "Tasks");
        }
    }
}
