using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "ESSFileNumbers",
                startValue: 100000L);

            migrationBuilder.CreateSequence(
                name: "seq_ReferralIds",
                startValue: 1000001L);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryCode = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryCode);
                });

            migrationBuilder.CreateTable(
                name: "FamilyRelationshipTypes",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyRelationshipTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
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
                name: "Communities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    RegionName = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communities_Regions_RegionName",
                        column: x => x.RegionName,
                        principalTable: "Regions",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncidentTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TaskNumber = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    RegionName = table.Column<string>(nullable: true),
                    CommunityId = table.Column<Guid>(nullable: true),
                    StartDate = table.Column<DateTimeOffset>(nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
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
                        name: "FK_IncidentTasks_Regions_RegionName",
                        column: x => x.RegionName,
                        principalTable: "Regions",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    BceidAccountNumber = table.Column<string>(maxLength: 100, nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    CommunityId = table.Column<Guid>(nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organizations_Regions_RegionName",
                        column: x => x.RegionName,
                        principalTable: "Regions",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvacueeRegistrations",
                columns: table => new
                {
                    EssFileNumber = table.Column<long>(nullable: false, defaultValueSql: "NEXT VALUE FOR ESSFileNumbers"),
                    Active = table.Column<bool>(nullable: false),
                    RestrictedAccess = table.Column<bool>(nullable: true),
                    DeclarationAndConsent = table.Column<bool>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberAlt = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DietaryNeeds = table.Column<bool>(nullable: true),
                    DietaryNeedsDetails = table.Column<string>(nullable: true),
                    DisasterAffectDetails = table.Column<string>(nullable: true),
                    ExternalReferralsDetails = table.Column<string>(nullable: true),
                    Facility = table.Column<string>(nullable: true),
                    FamilyRecoveryPlan = table.Column<string>(nullable: true),
                    FollowUpDetails = table.Column<string>(nullable: true),
                    InsuranceCode = table.Column<string>(nullable: true),
                    MedicationNeeds = table.Column<bool>(nullable: true),
                    SelfRegisteredDate = table.Column<DateTime>(nullable: true),
                    RegistrationCompletionDate = table.Column<DateTime>(nullable: true),
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
                    IncidentTaskId = table.Column<Guid>(nullable: true),
                    HostCommunityId = table.Column<Guid>(nullable: true),
                    CompletedById = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvacueeRegistrations", x => x.EssFileNumber);
                    table.ForeignKey(
                        name: "FK_EvacueeRegistrations_Communities_HostCommunityId",
                        column: x => x.HostCommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvacueeRegistrations_IncidentTasks_IncidentTaskId",
                        column: x => x.IncidentTaskId,
                        principalTable: "IncidentTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    BceidAccountUserName = table.Column<string>(nullable: true),
                    IsNewUser = table.Column<bool>(nullable: true),
                    IsAdministrator = table.Column<bool>(nullable: true),
                    IsPrimaryContact = table.Column<bool>(nullable: true),
                    CanAccessRestrictedFiles = table.Column<bool>(nullable: true),
                    UserId = table.Column<string>(maxLength: 255, nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
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

            migrationBuilder.CreateTable(
                name: "EvacueeRegistrationAddresses",
                columns: table => new
                {
                    RegistrationId = table.Column<long>(nullable: false),
                    AddressSequenceNumber = table.Column<int>(nullable: false),
                    AddressTypeCode = table.Column<string>(nullable: true),
                    AddressSubtypeCode = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    AddressLine3 = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    CommunityId = table.Column<Guid>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvacueeRegistrationAddresses", x => new { x.RegistrationId, x.AddressSequenceNumber });
                    table.ForeignKey(
                        name: "FK_EvacueeRegistrationAddresses_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvacueeRegistrationAddresses_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvacueeRegistrationAddresses_EvacueeRegistrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "EvacueeRegistrations",
                        principalColumn: "EssFileNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvacueeRegistrationAudits",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EssFileNumber = table.Column<long>(nullable: false),
                    Action = table.Column<string>(maxLength: 100, nullable: false),
                    Content = table.Column<string>(nullable: false),
                    User = table.Column<string>(maxLength: 100, nullable: false),
                    UserName = table.Column<string>(maxLength: 100, nullable: false),
                    UserType = table.Column<string>(maxLength: 100, nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvacueeRegistrationAudits", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_EvacueeRegistrationAudits_EvacueeRegistrations_EssFileNumber",
                        column: x => x.EssFileNumber,
                        principalTable: "EvacueeRegistrations",
                        principalColumn: "EssFileNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evacuees",
                columns: table => new
                {
                    RegistrationId = table.Column<long>(nullable: false),
                    EvacueeSequenceNumber = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 255, nullable: true),
                    LastName = table.Column<string>(maxLength: 255, nullable: true),
                    Nickname = table.Column<string>(maxLength: 255, nullable: true),
                    Initials = table.Column<string>(maxLength: 255, nullable: true),
                    SameLastNameAsEvacuee = table.Column<bool>(nullable: false),
                    Gender = table.Column<string>(maxLength: 255, nullable: true),
                    Dob = table.Column<DateTime>(nullable: true),
                    BcServicesNumber = table.Column<string>(nullable: true),
                    EvacueeTypeCode = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evacuees", x => new { x.RegistrationId, x.EvacueeSequenceNumber });
                    table.ForeignKey(
                        name: "FK_Evacuees_EvacueeRegistrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "EvacueeRegistrations",
                        principalColumn: "EssFileNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Referrals",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "NEXT VALUE FOR seq_ReferralIds"),
                    Active = table.Column<bool>(nullable: false),
                    RegistrationId = table.Column<long>(nullable: false),
                    ValidFrom = table.Column<DateTimeOffset>(nullable: false),
                    ValidTo = table.Column<DateTimeOffset>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    ConfirmChecked = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    Purchaser = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedByUserId = table.Column<string>(maxLength: 255, nullable: false, defaultValueSql: "'System'"),
                    NumberOfNights = table.Column<int>(nullable: true),
                    ExtremeWinterConditions = table.Column<bool>(nullable: true),
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
                    EvacueeId = table.Column<int>(nullable: false)
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
                name: "IX_Communities_RegionName",
                table: "Communities",
                column: "RegionName");

            migrationBuilder.CreateIndex(
                name: "IX_EvacueeRegistrationAddresses_CommunityId",
                table: "EvacueeRegistrationAddresses",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_EvacueeRegistrationAddresses_CountryCode",
                table: "EvacueeRegistrationAddresses",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_EvacueeRegistrationAudits_EssFileNumber",
                table: "EvacueeRegistrationAudits",
                column: "EssFileNumber");

            migrationBuilder.CreateIndex(
                name: "IX_EvacueeRegistrations_HostCommunityId",
                table: "EvacueeRegistrations",
                column: "HostCommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_EvacueeRegistrations_IncidentTaskId",
                table: "EvacueeRegistrations",
                column: "IncidentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentTasks_CommunityId",
                table: "IncidentTasks",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentTasks_RegionName",
                table: "IncidentTasks",
                column: "RegionName");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CommunityId",
                table: "Organizations",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_RegionName",
                table: "Organizations",
                column: "RegionName");

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

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_OrganizationId",
                table: "Volunteers",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvacueeRegistrationAddresses");

            migrationBuilder.DropTable(
                name: "EvacueeRegistrationAudits");

            migrationBuilder.DropTable(
                name: "FamilyRelationshipTypes");

            migrationBuilder.DropTable(
                name: "ReferralEvacuees");

            migrationBuilder.DropTable(
                name: "Volunteers");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Referrals");

            migrationBuilder.DropTable(
                name: "Evacuees");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "EvacueeRegistrations");

            migrationBuilder.DropTable(
                name: "IncidentTasks");

            migrationBuilder.DropTable(
                name: "Communities");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropSequence(
                name: "ESSFileNumbers");

            migrationBuilder.DropSequence(
                name: "seq_ReferralIds");
        }
    }
}
