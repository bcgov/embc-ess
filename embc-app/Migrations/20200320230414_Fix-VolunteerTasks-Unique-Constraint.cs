using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
	public partial class FixVolunteerTasksUniqueConstraint : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// drop existing indexes if they exist
			migrationBuilder.Sql(@"
				IF EXISTS(
					SELECT * 
					FROM sys.indexes 
					WHERE name='AK_VolunteerTasks_IncidentTaskId' AND object_id = OBJECT_ID('VolunteerTasks'))
				BEGIN
					ALTER TABLE [dbo].[VolunteerTasks] DROP CONSTRAINT [AK_VolunteerTasks_IncidentTaskId]
				END

				IF EXISTS(
					SELECT * 
					FROM sys.indexes 
					WHERE name='AK_VolunteerTasks_VolunteerId' AND object_id = OBJECT_ID('VolunteerTasks'))
				BEGIN
					ALTER TABLE [dbo].[VolunteerTasks] DROP CONSTRAINT [AK_VolunteerTasks_VolunteerId]
				END
			");
			// Create new unique index on both columns
			migrationBuilder.CreateIndex(
				name: "IX_VolunteerTasks_VolunteerId_IncidentTaskId",
				table: "VolunteerTasks",
				columns: new[] { "VolunteerId", "IncidentTaskId" },
				unique: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_VolunteerTasks_VolunteerId_IncidentTaskId",
				table: "VolunteerTasks");
		}
	}
}
