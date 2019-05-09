using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class SetCommunityRegionNameNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Regions_RegionName",
                table: "Communities");

            migrationBuilder.AlterColumn<string>(
                name: "RegionName",
                table: "Communities",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Regions_RegionName",
                table: "Communities",
                column: "RegionName",
                principalTable: "Regions",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Regions_RegionName",
                table: "Communities");

            migrationBuilder.AlterColumn<string>(
                name: "RegionName",
                table: "Communities",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Regions_RegionName",
                table: "Communities",
                column: "RegionName",
                principalTable: "Regions",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
