using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class SetEvacueeAddressCommunityIdnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvacueeRegistrationAddresses_Communities_CommunityId",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.AlterColumn<Guid>(
                name: "CommunityId",
                table: "EvacueeRegistrationAddresses",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_EvacueeRegistrationAddresses_Communities_CommunityId",
                table: "EvacueeRegistrationAddresses",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvacueeRegistrationAddresses_Communities_CommunityId",
                table: "EvacueeRegistrationAddresses");

            migrationBuilder.AlterColumn<Guid>(
                name: "CommunityId",
                table: "EvacueeRegistrationAddresses",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EvacueeRegistrationAddresses_Communities_CommunityId",
                table: "EvacueeRegistrationAddresses",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
