using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class ChangeRegionPrimaryKeyToName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "Organizations",
                nullable: true);

            migrationBuilder.Sql("UPDATE Organizations SET RegionName = r.Name FROM Organizations o INNER JOIN Regions r ON o.RegionId = r.Id");

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "IncidentTasks",
                nullable: true);

            migrationBuilder.Sql("UPDATE IncidentTasks SET RegionName = r.Name FROM IncidentTasks i INNER JOIN Regions r ON i.RegionId = r.Id");

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "Communities",
                nullable: true);

            migrationBuilder.Sql("UPDATE Communities SET RegionName = r.Name FROM Communities c INNER JOIN Regions r ON c.RegionId = r.Id");

            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Regions_RegionId",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentTasks_Regions_RegionId",
                table: "IncidentTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Regions_RegionId",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regions",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_RegionId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_IncidentTasks_RegionId",
                table: "IncidentTasks");

            migrationBuilder.DropIndex(
                name: "IX_Communities_RegionId",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "IncidentTasks");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Communities");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Regions",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regions",
                table: "Regions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_RegionName",
                table: "Organizations",
                column: "RegionName");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentTasks_RegionName",
                table: "IncidentTasks",
                column: "RegionName");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_RegionName",
                table: "Communities",
                column: "RegionName");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Regions_RegionName",
                table: "Communities",
                column: "RegionName",
                principalTable: "Regions",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentTasks_Regions_RegionName",
                table: "IncidentTasks",
                column: "RegionName",
                principalTable: "Regions",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Regions_RegionName",
                table: "Organizations",
                column: "RegionName",
                principalTable: "Regions",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Regions_RegionName",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentTasks_Regions_RegionName",
                table: "IncidentTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Regions_RegionName",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regions",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_RegionName",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_IncidentTasks_RegionName",
                table: "IncidentTasks");

            migrationBuilder.DropIndex(
                name: "IX_Communities_RegionName",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "IncidentTasks");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "Communities");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Regions",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Regions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                table: "IncidentTasks",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                table: "Communities",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regions",
                table: "Regions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_RegionId",
                table: "Organizations",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentTasks_RegionId",
                table: "IncidentTasks",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_RegionId",
                table: "Communities",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Regions_RegionId",
                table: "Communities",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentTasks_Regions_RegionId",
                table: "IncidentTasks",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Regions_RegionId",
                table: "Organizations",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
