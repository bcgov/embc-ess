using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class ReferralsPurchaser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPurchaser",
                table: "ReferralEvacuees");

            migrationBuilder.AddColumn<string>(
                name: "Purchaser",
                table: "Referrals",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Purchaser",
                table: "Referrals");

            migrationBuilder.AddColumn<bool>(
                name: "IsPurchaser",
                table: "ReferralEvacuees",
                nullable: false,
                defaultValue: false);
        }
    }
}
