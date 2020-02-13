using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class AddTaskNumberDateFieldsToIncidentTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TaskNumberEndDate",
                table: "IncidentTasks",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TaskNumberStartDate",
                table: "IncidentTasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskNumberEndDate",
                table: "IncidentTasks");

            migrationBuilder.DropColumn(
                name: "TaskNumberStartDate",
                table: "IncidentTasks");
        }
    }
}
