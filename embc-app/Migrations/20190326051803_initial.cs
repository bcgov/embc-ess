using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyRelationshipTypes",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyRelationshipTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    BceidAccountNumber = table.Column<string>(maxLength: 255, nullable: true),
                    Externaluseridentifier = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionalDistricts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    RegionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalDistricts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegionalDistricts_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Communities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    RegionalDistrictId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communities_RegionalDistricts_RegionalDistrictId",
                        column: x => x.RegionalDistrictId,
                        principalTable: "RegionalDistricts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AddressSubtype = table.Column<string>(nullable: false),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    AddressLine3 = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    CommunityId = table.Column<Guid>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    CountryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncidentTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TaskNumber = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    RegionId = table.Column<Guid>(nullable: true),
                    RegionalDistrictId = table.Column<Guid>(nullable: true),
                    CommunityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentTasks_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidentTasks_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidentTasks_RegionalDistricts_RegionalDistrictId",
                        column: x => x.RegionalDistrictId,
                        principalTable: "RegionalDistricts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    PersonType = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 255, nullable: true),
                    LastName = table.Column<string>(maxLength: 255, nullable: true),
                    Nickname = table.Column<string>(maxLength: 255, nullable: true),
                    Initials = table.Column<string>(maxLength: 255, nullable: true),
                    Gender = table.Column<string>(maxLength: 255, nullable: true),
                    Dob = table.Column<DateTimeOffset>(nullable: true),
                    BcServicesNumber = table.Column<string>(nullable: true),
                    SameLastNameAsEvacuee = table.Column<bool>(nullable: true),
                    RelationshipToEvacueeCode = table.Column<string>(nullable: true),
                    HeadOfHouseholdId = table.Column<Guid>(nullable: true),
                    HeadOfHousehold_BcServicesNumber = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberAlt = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PrimaryResidenceId = table.Column<Guid>(nullable: true),
                    MailingAddressId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Volunteer_Email = table.Column<string>(maxLength: 255, nullable: true),
                    BceidAccountNumber = table.Column<string>(nullable: true),
                    IsNewUser = table.Column<bool>(nullable: true),
                    IsAdministrator = table.Column<bool>(nullable: true),
                    IsPrimaryContact = table.Column<bool>(nullable: true),
                    CanAccessRestrictedFiles = table.Column<bool>(nullable: true),
                    Externaluseridentifier = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_People_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    RestrictedAccess = table.Column<bool>(nullable: true),
                    DeclarationAndConsent = table.Column<bool>(nullable: true),
                    EssFileNumber = table.Column<long>(nullable: true),
                    DietaryNeeds = table.Column<bool>(nullable: true),
                    DietaryNeedsDetails = table.Column<string>(nullable: true),
                    DisasterAffectDetails = table.Column<string>(nullable: true),
                    ExternalReferralsDetails = table.Column<string>(nullable: true),
                    Facility = table.Column<string>(nullable: true),
                    FamilyRecoveryPlan = table.Column<string>(nullable: true),
                    FollowUpDetails = table.Column<string>(nullable: true),
                    InsuranceCode = table.Column<string>(nullable: true),
                    MedicationNeeds = table.Column<bool>(nullable: true),
                    SelfRegisteredDate = table.Column<DateTimeOffset>(nullable: true),
                    RegistrationCompletionDate = table.Column<DateTimeOffset>(nullable: true),
                    RegisteringFamilyMembers = table.Column<string>(nullable: true),
                    HasThreeDayMedicationSupply = table.Column<bool>(nullable: true),
                    HasInquiryReferral = table.Column<bool>(nullable: true),
                    HasHealthServicesReferral = table.Column<bool>(nullable: true),
                    HasFirstAidReferral = table.Column<bool>(nullable: true),
                    HasChildCareReferral = table.Column<bool>(nullable: true),
                    HasPersonalServicesReferral = table.Column<bool>(nullable: true),
                    HasPetCareReferral = table.Column<bool>(nullable: true),
                    HasPets = table.Column<bool>(nullable: true),
                    RequiresAccommodation = table.Column<bool>(nullable: true),
                    RequiresClothing = table.Column<bool>(nullable: true),
                    RequiresFood = table.Column<bool>(nullable: true),
                    RequiresIncidentals = table.Column<bool>(nullable: true),
                    RequiresTransportation = table.Column<bool>(nullable: true),
                    RequiresSupport = table.Column<bool>(nullable: true),
                    HeadOfHouseholdId = table.Column<Guid>(nullable: true),
                    IncidentTaskId = table.Column<Guid>(nullable: true),
                    HostCommunityId = table.Column<Guid>(nullable: true)
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
                name: "IX_Addresses_CommunityId",
                table: "Addresses",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_RegionalDistrictId",
                table: "Communities",
                column: "RegionalDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentTasks_CommunityId",
                table: "IncidentTasks",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentTasks_RegionId",
                table: "IncidentTasks",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentTasks_RegionalDistrictId",
                table: "IncidentTasks",
                column: "RegionalDistrictId");

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
                name: "IX_People_OrganizationId",
                table: "People",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionalDistricts_RegionId",
                table: "RegionalDistricts",
                column: "RegionId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "IncidentTasks");

            migrationBuilder.DropTable(
                name: "FamilyRelationshipTypes");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Communities");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "RegionalDistricts");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
