using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class RemoveRegionalDistrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_RegionalDistricts_RegionalDistrictId",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentTasks_RegionalDistricts_RegionalDistrictId",
                table: "IncidentTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_RegionalDistricts_RegionalDistrictId",
                table: "Organizations");

            migrationBuilder.Sql(@"UPDATE Communities
	                                                SET RegionalDistrictId = rd.RegionId
                                                FROM
	                                                Communities c
                                                INNER JOIN
	                                                RegionalDistricts rd
	                                                ON c.RegionalDistrictId = rd.Id");


            migrationBuilder.DropTable(
                name: "RegionalDistricts");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_RegionalDistrictId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_IncidentTasks_RegionalDistrictId",
                table: "IncidentTasks");

            migrationBuilder.DropColumn(
                name: "RegionalDistrictId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "RegionalDistrictId",
                table: "IncidentTasks");

            migrationBuilder.RenameColumn(
                name: "RegionalDistrictId",
                table: "Communities",
                newName: "RegionId");


            migrationBuilder.RenameIndex(
                name: "IX_Communities_RegionalDistrictId",
                table: "Communities",
                newName: "IX_Communities_RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Regions_RegionId",
                table: "Communities",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Regions_RegionId",
                table: "Communities");

            migrationBuilder.Sql("UPDATE Communities SET RegionId = NULL");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Communities",
                newName: "RegionalDistrictId");

            migrationBuilder.RenameIndex(
                name: "IX_Communities_RegionId",
                table: "Communities",
                newName: "IX_Communities_RegionalDistrictId");

            migrationBuilder.AddColumn<Guid>(
                name: "RegionalDistrictId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RegionalDistrictId",
                table: "IncidentTasks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RegionalDistricts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    RegionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalDistricts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegionalDistricts_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_RegionalDistrictId",
                table: "Organizations",
                column: "RegionalDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentTasks_RegionalDistrictId",
                table: "IncidentTasks",
                column: "RegionalDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionalDistricts_RegionId",
                table: "RegionalDistricts",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_RegionalDistricts_RegionalDistrictId",
                table: "Communities",
                column: "RegionalDistrictId",
                principalTable: "RegionalDistricts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentTasks_RegionalDistricts_RegionalDistrictId",
                table: "IncidentTasks",
                column: "RegionalDistrictId",
                principalTable: "RegionalDistricts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_RegionalDistricts_RegionalDistrictId",
                table: "Organizations",
                column: "RegionalDistrictId",
                principalTable: "RegionalDistricts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
