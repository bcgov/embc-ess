using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class PetCarePlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PetCarePlan",
                table: "EvacueeRegistrations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PetCarePlan",
                table: "EvacueeRegistrations");
        }
    }
}
