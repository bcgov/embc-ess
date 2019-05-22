using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class OldRegistrationsCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Addresses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    AddressLine3 = table.Column<string>(nullable: true),
                    AddressSubtype = table.Column<string>(nullable: false),
                    CountryCode = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    PostalCode = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CommunityId = table.Column<Guid>(nullable: true),
                    City = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Dob = table.Column<DateTimeOffset>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 255, nullable: true),
                    Gender = table.Column<string>(maxLength: 255, nullable: true),
                    Initials = table.Column<string>(maxLength: 255, nullable: true),
                    LastName = table.Column<string>(maxLength: 255, nullable: true),
                    Nickname = table.Column<string>(maxLength: 255, nullable: true),
                    PersonType = table.Column<string>(nullable: false),
                    BcServicesNumber = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: true, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    HeadOfHouseholdId = table.Column<Guid>(nullable: true),
                    RelationshipToEvacueeCode = table.Column<string>(nullable: true),
                    SameLastNameAsEvacuee = table.Column<bool>(nullable: true),
                    UpdateDateTime = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: true, defaultValueSql: "'System'"),
                    HeadOfHousehold_BcServicesNumber = table.Column<string>(nullable: true),
                    HeadOfHousehold_CreatedByUserId = table.Column<string>(maxLength: 255, nullable: true, defaultValueSql: "'System'"),
                    HeadOfHousehold_CreatedDateTime = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    Email = table.Column<string>(nullable: true),
                    MailingAddressId = table.Column<Guid>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberAlt = table.Column<string>(nullable: true),
                    PrimaryResidenceId = table.Column<Guid>(nullable: true),
                    HeadOfHousehold_UpdateDateTime = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    HeadOfHousehold_UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: true, defaultValueSql: "'System'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_People_HeadOfHouseholdId",
                        column: x => x.HeadOfHouseholdId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_FamilyRelationshipTypes_RelationshipToEvacueeCode",
                        column: x => x.RelationshipToEvacueeCode,
                        principalTable: "FamilyRelationshipTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_Addresses_MailingAddressId",
                        column: x => x.MailingAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_Addresses_PrimaryResidenceId",
                        column: x => x.PrimaryResidenceId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CompletedById = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    DeclarationAndConsent = table.Column<bool>(nullable: true),
                    DietaryNeeds = table.Column<bool>(nullable: true),
                    DietaryNeedsDetails = table.Column<string>(nullable: true),
                    DisasterAffectDetails = table.Column<string>(nullable: true),
                    EssFileNumber = table.Column<long>(nullable: false, defaultValueSql: "NEXT VALUE FOR ESSFileNumbers"),
                    ExternalReferralsDetails = table.Column<string>(nullable: true),
                    Facility = table.Column<string>(nullable: true),
                    FamilyRecoveryPlan = table.Column<string>(nullable: true),
                    FollowUpDetails = table.Column<string>(nullable: true),
                    HasChildCareReferral = table.Column<bool>(nullable: true),
                    HasFirstAidReferral = table.Column<bool>(nullable: true),
                    HasHealthServicesReferral = table.Column<bool>(nullable: true),
                    HasInquiryReferral = table.Column<bool>(nullable: true),
                    HasPersonalServicesReferral = table.Column<bool>(nullable: true),
                    HasPetCareReferral = table.Column<bool>(nullable: true),
                    HasPets = table.Column<bool>(nullable: true),
                    HasThreeDayMedicationSupply = table.Column<bool>(nullable: true),
                    HeadOfHouseholdId = table.Column<Guid>(nullable: true),
                    HostCommunityId = table.Column<Guid>(nullable: true),
                    IncidentTaskId = table.Column<Guid>(nullable: true),
                    InsuranceCode = table.Column<string>(nullable: true),
                    MedicationNeeds = table.Column<bool>(nullable: true),
                    RegisteringFamilyMembers = table.Column<string>(nullable: true),
                    RegistrationCompletionDate = table.Column<DateTimeOffset>(nullable: true),
                    RequiresAccommodation = table.Column<bool>(nullable: true),
                    RequiresClothing = table.Column<bool>(nullable: true),
                    RequiresFood = table.Column<bool>(nullable: true),
                    RequiresIncidentals = table.Column<bool>(nullable: true),
                    RequiresSupport = table.Column<bool>(nullable: true),
                    RequiresTransportation = table.Column<bool>(nullable: true),
                    RestrictedAccess = table.Column<bool>(nullable: true),
                    SelfRegisteredDate = table.Column<DateTimeOffset>(nullable: true),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrations_People_HeadOfHouseholdId",
                        column: x => x.HeadOfHouseholdId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_Communities_HostCommunityId",
                        column: x => x.HostCommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registrations_IncidentTasks_IncidentTaskId",
                        column: x => x.IncidentTaskId,
                        principalTable: "IncidentTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryCode",
                table: "Addresses",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CommunityId",
                table: "Addresses",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_People_HeadOfHouseholdId",
                table: "People",
                column: "HeadOfHouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_People_RelationshipToEvacueeCode",
                table: "People",
                column: "RelationshipToEvacueeCode");

            migrationBuilder.CreateIndex(
                name: "IX_People_MailingAddressId",
                table: "People",
                column: "MailingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_People_PrimaryResidenceId",
                table: "People",
                column: "PrimaryResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_HeadOfHouseholdId",
                table: "Registrations",
                column: "HeadOfHouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_HostCommunityId",
                table: "Registrations",
                column: "HostCommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_IncidentTaskId",
                table: "Registrations",
                column: "IncidentTaskId");
        }
    }
}
