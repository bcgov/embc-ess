using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class DropAlternateKeyEvacueeRegistrationAddresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Evacuees_EvacueeSequenceNumber_RegistrationId",
                table: "Evacuees");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_EvacueeRegistrationAddresses_AddressSequenceNumber_RegistrationId",
                table: "EvacueeRegistrationAddresses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Evacuees_EvacueeSequenceNumber_RegistrationId",
                table: "Evacuees",
                columns: new[] { "EvacueeSequenceNumber", "RegistrationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_EvacueeRegistrationAddresses_AddressSequenceNumber_RegistrationId",
                table: "EvacueeRegistrationAddresses",
                columns: new[] { "AddressSequenceNumber", "RegistrationId" });
        }
    }
}
