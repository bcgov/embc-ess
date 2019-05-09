using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class MakeVolunteerIAuditableEntityIDMax255 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Volunteers",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Volunteers",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Volunteers",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Volunteers",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Registrations",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Registrations",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Regions",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Regions",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "HeadOfHousehold_UpdatedByUserId",
                table: "People",
                maxLength: 255,
                nullable: true,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "HeadOfHousehold_CreatedByUserId",
                table: "People",
                maxLength: 255,
                nullable: true,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "People",
                maxLength: 255,
                nullable: true,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "People",
                maxLength: 255,
                nullable: true,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Organizations",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Organizations",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "IncidentTasks",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "IncidentTasks",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Evacuees",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Evacuees",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "EvacueeRegistrations",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "EvacueeRegistrations",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "EvacueeRegistrationAddresses",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "EvacueeRegistrationAddresses",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Countries",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Countries",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Communities",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Communities",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Addresses",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Addresses",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldDefaultValueSql: "'System'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Volunteers");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Registrations",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Registrations",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Regions",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Regions",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "HeadOfHousehold_UpdatedByUserId",
                table: "People",
                nullable: true,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "HeadOfHousehold_CreatedByUserId",
                table: "People",
                nullable: true,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "People",
                nullable: true,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "People",
                nullable: true,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Organizations",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Organizations",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "IncidentTasks",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "IncidentTasks",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Evacuees",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Evacuees",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "EvacueeRegistrations",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "EvacueeRegistrations",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "EvacueeRegistrationAddresses",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "EvacueeRegistrationAddresses",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Countries",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Countries",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Communities",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Communities",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "Addresses",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Addresses",
                nullable: false,
                defaultValueSql: "'System'",
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldDefaultValueSql: "'System'");
        }
    }
}
