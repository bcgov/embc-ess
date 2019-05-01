using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class VolunterTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Organizations_OrganizationId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_People_CompletedById",
                table: "Registrations");

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 255, nullable: true),
                    LastName = table.Column<string>(maxLength: 255, nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    BceidAccountNumber = table.Column<string>(nullable: true),
                    IsNewUser = table.Column<bool>(nullable: true),
                    IsAdministrator = table.Column<bool>(nullable: true),
                    IsPrimaryContact = table.Column<bool>(nullable: true),
                    CanAccessRestrictedFiles = table.Column<bool>(nullable: true),
                    Externaluseridentifier = table.Column<Guid>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    PeopleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Volunteers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //Copy the data
            migrationBuilder.Sql(@"INSERT INTO Volunteers
                                                    (FirstName
                                                    ,LastName
                                                    ,Email
                                                    ,Active
                                                    ,BceidAccountNumber
                                                    ,IsNewUser
                                                    ,IsAdministrator
                                                    ,IsPrimaryContact
                                                    ,CanAccessRestrictedFiles
                                                    ,Externaluseridentifier
                                                    ,OrganizationId
                                                    ,PeopleId)
                                                SELECT 
                                                    FirstName
                                                    ,LastName
                                                    ,Email
                                                    ,Active
                                                    ,BceidAccountNumber
                                                    ,IsNewUser
                                                    ,IsAdministrator
                                                    ,IsPrimaryContact
                                                    ,CanAccessRestrictedFiles
                                                    ,Externaluseridentifier
                                                    ,OrganizationId
                                                    ,p.Id
                                                FROM
	                                                People p
                                                WHERE
	                                                p.PersonType = 'VOLN'");


            //Migrate values from new table but use the new ForeignKey resource Externaluseridentifier
            //Add temp column
            migrationBuilder.AddColumn<Guid>(
                name: "CompletedByPeopleId",
                table: "Registrations",
                nullable: true);

            migrationBuilder.Sql("UPDATE Registrations SET CompletedByPeopleId = CompletedById");
            migrationBuilder.Sql("UPDATE Registrations SET CompletedById = null");


            migrationBuilder.DropIndex(
                name: "IX_Registrations_CompletedById",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "CompletedById",
                table: "Registrations");

            migrationBuilder.AddColumn<int>(
                name: "CompletedById",
                table: "Registrations",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE Registrations
                                                    SET CompletedById = v.Id
                                                FROM Registrations r
                                                INNER JOIN
                                                    Volunteers v
                                                    ON r.CompletedByPeopleId = v.PeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_CompletedById",
                table: "Registrations",
                column: "CompletedById");

            migrationBuilder.DropColumn(
                name: "CompletedByPeopleId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "PeopleId",
                table: "Volunteers");

            migrationBuilder.DropIndex(
                name: "IX_People_OrganizationId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "People");

            migrationBuilder.DropColumn(
                name: "BceidAccountNumber",
                table: "People");

            migrationBuilder.DropColumn(
                name: "CanAccessRestrictedFiles",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Volunteer_Email",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Externaluseridentifier",
                table: "People");

            migrationBuilder.DropColumn(
                name: "IsAdministrator",
                table: "People");

            migrationBuilder.DropColumn(
                name: "IsNewUser",
                table: "People");

            migrationBuilder.DropColumn(
                name: "IsPrimaryContact",
                table: "People");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "People");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_OrganizationId",
                table: "Volunteers",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Volunteers_CompletedById",
                table: "Registrations",
                column: "CompletedById",
                principalTable: "Volunteers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Volunteers_CompletedById",
                table: "Registrations");

            migrationBuilder.DropTable(
                name: "Volunteers");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompletedById",
                table: "Registrations",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "People",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BceidAccountNumber",
                table: "People",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanAccessRestrictedFiles",
                table: "People",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Volunteer_Email",
                table: "People",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Externaluseridentifier",
                table: "People",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdministrator",
                table: "People",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNewUser",
                table: "People",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimaryContact",
                table: "People",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "People",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_OrganizationId",
                table: "People",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Organizations_OrganizationId",
                table: "People",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_People_CompletedById",
                table: "Registrations",
                column: "CompletedById",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
