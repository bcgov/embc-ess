using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class IncidentTaskEvacueeSummaryView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW IncidentTaskEvacueeSummaryView AS
                    SELECT it.Id as IncidentTaskId, it.TaskNumber, COUNT(CASE WHEN e.RegistrationId IS NOT NULL THEN 1 END) TotalEvacuees
                FROM
	                IncidentTasks it
                LEFT JOIN
	                EvacueeRegistrations er
	                ON it.Id = er.IncidentTaskId
                LEFT JOIN
	                Evacuees e
	                ON er.EssFileNumber = e.RegistrationId
                GROUP BY it.Id, it.TaskNumber
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IncidentTaskEvacueeSummaryView");
        }
    }
}
