using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Najam.TaskBook.Data.Migrations
{
    public partial class AddingOverdueLogicToTaskEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_AspNetUsers_AssignedToUserId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "DateCompleted",
                table: "Tasks",
                newName: "DateTimeCompleted");

            migrationBuilder.AddColumn<bool>(
                name: "IsOverdue",
                table: "Tasks",
                nullable: false,
                computedColumnSql: "case when DateTimeCompleted is null and Deadline < getdate() then 1 else 0 end");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_AspNetUsersAssignedToUserId",
                table: "Tasks",
                column: "AssignedToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_AspNetUsersAssignedToUserId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IsOverdue",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "DateTimeCompleted",
                table: "Tasks",
                newName: "DateCompleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_AspNetUsers_AssignedToUserId",
                table: "Tasks",
                column: "AssignedToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
