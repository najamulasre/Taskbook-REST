using Microsoft.EntityFrameworkCore.Migrations;

namespace Najam.TaskBook.WebApi.Migrations
{
    public partial class FixingTypeOnTaskOverdueColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsOverdue",
                table: "Tasks",
                nullable: false,
                computedColumnSql: "case when DateTimeCompleted is null and Deadline < getdate() then cast(1 as bit) else cast(0 as bit) end",
                oldClrType: typeof(bool),
                oldComputedColumnSql: "case when DateTimeCompleted is null and Deadline < getdate() then 1 else 0 end");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsOverdue",
                table: "Tasks",
                nullable: false,
                computedColumnSql: "case when DateTimeCompleted is null and Deadline < getdate() then 1 else 0 end",
                oldClrType: typeof(bool),
                oldComputedColumnSql: "case when DateTimeCompleted is null and Deadline < getdate() then cast(1 as bit) else cast(0 as bit) end");
        }
    }
}
