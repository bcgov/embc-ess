using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class EvacueeRegistrationAuditInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvacueeRegistrationAudits",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EssFileNumber = table.Column<long>(nullable: false),
                    Action = table.Column<string>(maxLength: 100, nullable: false),
                    Content = table.Column<string>(nullable: false),
                    User = table.Column<string>(maxLength: 100, nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvacueeRegistrationAudits", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_EvacueeRegistrationAudits_EvacueeRegistrations_EssFileNumber",
                        column: x => x.EssFileNumber,
                        principalTable: "EvacueeRegistrations",
                        principalColumn: "EssFileNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvacueeRegistrationAudits_EssFileNumber",
                table: "EvacueeRegistrationAudits",
                column: "EssFileNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvacueeRegistrationAudits");
        }
    }
}
