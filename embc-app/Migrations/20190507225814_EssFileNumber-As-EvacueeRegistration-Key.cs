using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class EssFileNumberAsEvacueeRegistrationKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvacueeRegistrationAddresses_EvacueeRegistrations_EvacueeRegistrationId",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Evacuees_EvacueeRegistrations_EvacueeRegistrationId",
                table: "Evacuees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evacuees",
                table: "Evacuees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvacueeRegistrations",
                table: "EvacueeRegistrations");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_EvacueeRegistrationAddresses_AddressSequenceNumber_EvacueeRegistrationId",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvacueeRegistrationAddresses",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.AddColumn<long>(
                name: "RegistrationId",
                table: "Evacuees",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.Sql(@"UPDATE Evacuees
                                                SET RegistrationId = er.EssFileNumber
                                                FROM Evacuees e
                                                INNER JOIN EvacueeRegistrations er ON e.EvacueeRegistrationId = er.Id");

            migrationBuilder.AddColumn<long>(
                name: "RegistrationId",
                table: "EvacueeRegistrationAddresses",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.Sql(@"UPDATE EvacueeRegistrationAddresses
                                                SET RegistrationId = er.EssFileNumber
                                                FROM EvacueeRegistrationAddresses e
                                                INNER JOIN EvacueeRegistrations er ON e.EvacueeRegistrationId = er.Id");

            migrationBuilder.DropColumn(
                name: "EvacueeRegistrationId",
                table: "Evacuees");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EvacueeRegistrations");

            migrationBuilder.DropColumn(
                name: "EvacueeRegistrationId",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.AlterColumn<long>(
                name: "EssFileNumber",
                table: "EvacueeRegistrations",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR ESSFileNumbers",
                oldClrType: typeof(long));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Evacuees_EvacueeSequenceNumber_RegistrationId",
                table: "Evacuees",
                columns: new[] { "EvacueeSequenceNumber", "RegistrationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evacuees",
                table: "Evacuees",
                columns: new[] { "RegistrationId", "EvacueeSequenceNumber" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvacueeRegistrations",
                table: "EvacueeRegistrations",
                column: "EssFileNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_EvacueeRegistrationAddresses_AddressSequenceNumber_RegistrationId",
                table: "EvacueeRegistrationAddresses",
                columns: new[] { "AddressSequenceNumber", "RegistrationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvacueeRegistrationAddresses",
                table: "EvacueeRegistrationAddresses",
                columns: new[] { "RegistrationId", "AddressSequenceNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_EvacueeRegistrationAddresses_EvacueeRegistrations_RegistrationId",
                table: "EvacueeRegistrationAddresses",
                column: "RegistrationId",
                principalTable: "EvacueeRegistrations",
                principalColumn: "EssFileNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evacuees_EvacueeRegistrations_RegistrationId",
                table: "Evacuees",
                column: "RegistrationId",
                principalTable: "EvacueeRegistrations",
                principalColumn: "EssFileNumber",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvacueeRegistrationAddresses_EvacueeRegistrations_RegistrationId",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Evacuees_EvacueeRegistrations_RegistrationId",
                table: "Evacuees");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Evacuees_EvacueeSequenceNumber_RegistrationId",
                table: "Evacuees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evacuees",
                table: "Evacuees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvacueeRegistrations",
                table: "EvacueeRegistrations");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_EvacueeRegistrationAddresses_AddressSequenceNumber_RegistrationId",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvacueeRegistrationAddresses",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.DropColumn(
                name: "RegistrationId",
                table: "Evacuees");

            migrationBuilder.DropColumn(
                name: "RegistrationId",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.AddColumn<Guid>(
                name: "EvacueeRegistrationId",
                table: "Evacuees",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<long>(
                name: "EssFileNumber",
                table: "EvacueeRegistrations",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "NEXT VALUE FOR ESSFileNumbers");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "EvacueeRegistrations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EvacueeRegistrationId",
                table: "EvacueeRegistrationAddresses",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evacuees",
                table: "Evacuees",
                columns: new[] { "EvacueeRegistrationId", "EvacueeSequenceNumber" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvacueeRegistrations",
                table: "EvacueeRegistrations",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_EvacueeRegistrationAddresses_AddressSequenceNumber_EvacueeRegistrationId",
                table: "EvacueeRegistrationAddresses",
                columns: new[] { "AddressSequenceNumber", "EvacueeRegistrationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvacueeRegistrationAddresses",
                table: "EvacueeRegistrationAddresses",
                columns: new[] { "EvacueeRegistrationId", "AddressSequenceNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_EvacueeRegistrationAddresses_EvacueeRegistrations_EvacueeRegistrationId",
                table: "EvacueeRegistrationAddresses",
                column: "EvacueeRegistrationId",
                principalTable: "EvacueeRegistrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evacuees_EvacueeRegistrations_EvacueeRegistrationId",
                table: "Evacuees",
                column: "EvacueeRegistrationId",
                principalTable: "EvacueeRegistrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
