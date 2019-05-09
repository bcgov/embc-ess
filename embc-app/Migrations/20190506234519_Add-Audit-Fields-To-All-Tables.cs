using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class AddAuditFieldsToAllTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Registrations",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Registrations",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Registrations",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Registrations",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Regions",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Regions",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Regions",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Regions",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "People",
                nullable: true,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "People",
                nullable: true,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "People",
                nullable: true,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "People",
                nullable: true,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<string>(
                name: "HeadOfHousehold_CreatedByUserId",
                table: "People",
                nullable: true,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "HeadOfHousehold_CreatedDateTime",
                table: "People",
                nullable: true,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "HeadOfHousehold_UpdateDateTime",
                table: "People",
                nullable: true,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "HeadOfHousehold_UpdatedByUserId",
                table: "People",
                nullable: true,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Organizations",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Organizations",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Organizations",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Organizations",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "IncidentTasks",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "IncidentTasks",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "IncidentTasks",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "IncidentTasks",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Evacuees",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Evacuees",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Evacuees",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Evacuees",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "EvacueeRegistrations",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EvacueeRegistrations",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "EvacueeRegistrations",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "EvacueeRegistrations",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "EvacueeRegistrationAddresses",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EvacueeRegistrationAddresses",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "EvacueeRegistrationAddresses",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "EvacueeRegistrationAddresses",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Countries",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Countries",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Countries",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Countries",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Communities",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Communities",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Communities",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Communities",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Addresses",
                nullable: false,
                defaultValueSql: "'System'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Addresses",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "Addresses",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Addresses",
                nullable: false,
                defaultValueSql: "'System'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "People");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "People");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "HeadOfHousehold_CreatedByUserId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "HeadOfHousehold_CreatedDateTime",
                table: "People");

            migrationBuilder.DropColumn(
                name: "HeadOfHousehold_UpdateDateTime",
                table: "People");

            migrationBuilder.DropColumn(
                name: "HeadOfHousehold_UpdatedByUserId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "IncidentTasks");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "IncidentTasks");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "IncidentTasks");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "IncidentTasks");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Evacuees");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Evacuees");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "Evacuees");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Evacuees");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "EvacueeRegistrations");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EvacueeRegistrations");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "EvacueeRegistrations");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "EvacueeRegistrations");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Addresses");
        }
    }
}
