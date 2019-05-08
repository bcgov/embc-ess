using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class ReferralsInitialEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "seq_ReferralIds",
                startValue: 1000001L);

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Address = table.Column<string>(maxLength: 255, nullable: true),
                    City = table.Column<string>(maxLength: 255, nullable: true),
                    Province = table.Column<string>(maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 50, nullable: true),
                    Telephone = table.Column<string>(maxLength: 50, nullable: true),
                    Fax = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Referrals",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "NEXT VALUE FOR seq_ReferralIds"),
                    RegistrationId = table.Column<long>(nullable: false),
                    ValidFrom = table.Column<DateTimeOffset>(nullable: false),
                    ValidTo = table.Column<DateTimeOffset>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    ConfirmChecked = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    Type = table.Column<string>(maxLength: 15, nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    NumberOfNights = table.Column<int>(nullable: true),
                    ExtremeWeatherConditions = table.Column<bool>(nullable: true),
                    NumberOfMeals = table.Column<int>(nullable: true),
                    GroupLodgingReferral_NumberOfNights = table.Column<int>(nullable: true),
                    HotelLodgingReferral_NumberOfNights = table.Column<int>(nullable: true),
                    NumberOfRooms = table.Column<int>(nullable: true),
                    ApprovedItems = table.Column<string>(nullable: true),
                    TransportMode = table.Column<string>(maxLength: 255, nullable: true),
                    NumberOfBreakfasts = table.Column<int>(nullable: true),
                    NumberOfLunches = table.Column<int>(nullable: true),
                    NumberOfDinners = table.Column<int>(nullable: true),
                    FromAddress = table.Column<string>(maxLength: 255, nullable: true),
                    ToAddress = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referrals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Referrals_EvacueeRegistrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "EvacueeRegistrations",
                        principalColumn: "EssFileNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Referrals_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReferralEvacuees",
                columns: table => new
                {
                    ReferralId = table.Column<long>(nullable: false),
                    RegistrationId = table.Column<long>(nullable: false),
                    EvacueeId = table.Column<int>(nullable: false),
                    IsPurchaser = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferralEvacuees", x => new { x.RegistrationId, x.EvacueeId, x.ReferralId });
                    table.ForeignKey(
                        name: "FK_ReferralEvacuees_Referrals_ReferralId",
                        column: x => x.ReferralId,
                        principalTable: "Referrals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferralEvacuees_Evacuees_RegistrationId_EvacueeId",
                        columns: x => new { x.RegistrationId, x.EvacueeId },
                        principalTable: "Evacuees",
                        principalColumns: new[] { "RegistrationId", "EvacueeSequenceNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferralEvacuees_ReferralId",
                table: "ReferralEvacuees",
                column: "ReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_RegistrationId",
                table: "Referrals",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_SupplierId",
                table: "Referrals",
                column: "SupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferralEvacuees");

            migrationBuilder.DropTable(
                name: "Referrals");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropSequence(
                name: "seq_ReferralIds");
        }
    }
}
