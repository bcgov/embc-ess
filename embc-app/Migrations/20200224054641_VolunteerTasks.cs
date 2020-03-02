using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class VolunteerTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VolunteerTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VolunteerId = table.Column<int>(nullable: false),
                    IncidentTaskId = table.Column<Guid>(nullable: false),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerTasks", x => x.Id);
                    table.UniqueConstraint("AK_VolunteerTasks_IncidentTaskId", x => x.IncidentTaskId);
                    table.UniqueConstraint("AK_VolunteerTasks_VolunteerId", x => x.VolunteerId);
                    table.ForeignKey(
                        name: "FK_VolunteerTasks_IncidentTasks_IncidentTaskId",
                        column: x => x.IncidentTaskId,
                        principalTable: "IncidentTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VolunteerTasks_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VolunteerTasks");
        }
    }
}
