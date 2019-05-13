using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class EvacueeRegistrationEvacueeEvacueeAddressTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvacueeRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    RestrictedAccess = table.Column<bool>(nullable: true),
                    DeclarationAndConsent = table.Column<bool>(nullable: true),
                    EssFileNumber = table.Column<long>(nullable: false),
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
                    CompletedById = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvacueeRegistrations", x => x.Id);
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
                name: "EvacueeRegistrationAddresses",
                columns: table => new
                {
                    EvacueeRegistrationId = table.Column<Guid>(nullable: false),
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
                    CountryCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvacueeRegistrationAddresses", x => new { x.EvacueeRegistrationId, x.AddressSequenceNumber });
                    table.UniqueConstraint("AK_EvacueeRegistrationAddresses_AddressSequenceNumber_EvacueeRegistrationId", x => new { x.AddressSequenceNumber, x.EvacueeRegistrationId });
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
                        name: "FK_EvacueeRegistrationAddresses_EvacueeRegistrations_EvacueeRegistrationId",
                        column: x => x.EvacueeRegistrationId,
                        principalTable: "EvacueeRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evacuees",
                columns: table => new
                {
                    EvacueeRegistrationId = table.Column<Guid>(nullable: false),
                    EvacueeSequenceNumber = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 255, nullable: true),
                    LastName = table.Column<string>(maxLength: 255, nullable: true),
                    Nickname = table.Column<string>(maxLength: 255, nullable: true),
                    Initials = table.Column<string>(maxLength: 255, nullable: true),
                    SameLastNameAsEvacuee = table.Column<bool>(nullable: false),
                    Gender = table.Column<string>(maxLength: 255, nullable: true),
                    Dob = table.Column<DateTime>(nullable: true),
                    BcServicesNumber = table.Column<string>(nullable: true),
                    EvacueeTypeCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evacuees", x => new { x.EvacueeRegistrationId, x.EvacueeSequenceNumber });
                    table.ForeignKey(
                        name: "FK_Evacuees_EvacueeRegistrations_EvacueeRegistrationId",
                        column: x => x.EvacueeRegistrationId,
                        principalTable: "EvacueeRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"INSERT INTO [dbo].[EvacueeRegistrations]
												(Id,
												Active,
												RestrictedAccess,
												DeclarationAndConsent,
												EssFileNumber,
												PhoneNumber,
												PhoneNumberAlt,
												Email,
												DietaryNeeds,
												DietaryNeedsDetails,
												DisasterAffectDetails,
												ExternalReferralsDetails,
												Facility,
												FamilyRecoveryPlan,
												FollowUpDetails,
												InsuranceCode,
												MedicationNeeds,
												SelfRegisteredDate,
												RegistrationCompletionDate,
												RegisteringFamilyMembers,
												HasThreeDayMedicationSupply,
												HasInquiryReferral,
												HasHealthServicesReferral,
												HasFirstAidReferral,
												HasChildCareReferral,
												HasPersonalServicesReferral,
												HasPetCareReferral,
												HasPets,
												RequiresAccommodation,
												RequiresClothing,
												RequiresFood,
												RequiresIncidentals,
												RequiresTransportation,
												RequiresSupport,
												IncidentTaskId,
												HostCommunityId,
												CompletedById)
											SELECT
												r.Id,
												r.Active,
												r.RestrictedAccess,
												r.DeclarationAndConsent,
												r.EssFileNumber,
												p.PhoneNumber,
												p.PhoneNumberAlt,
												p.Email,
												r.DietaryNeeds,
												r.DietaryNeedsDetails,
												r.DisasterAffectDetails,
												r.ExternalReferralsDetails,
												r.Facility,
												r.FamilyRecoveryPlan,
												r.FollowUpDetails,
												r.InsuranceCode,
												r.MedicationNeeds,
												r.SelfRegisteredDate,
												r.RegistrationCompletionDate,
												r.RegisteringFamilyMembers,
												r.HasThreeDayMedicationSupply,
												r.HasInquiryReferral,
												r.HasHealthServicesReferral,
												r.HasFirstAidReferral,
												r.HasChildCareReferral,
												r.HasPersonalServicesReferral,
												r.HasPetCareReferral,
												r.HasPets,
												r.RequiresAccommodation,
												r.RequiresClothing,
												r.RequiresFood,
												r.RequiresIncidentals,
												r.RequiresTransportation,
												r.RequiresSupport,
												r.IncidentTaskId,
												r.HostCommunityId,
												r.CompletedById
											FROM
												Registrations r
											INNER JOIN
												People p
												ON r.HeadOfHouseholdId = p.Id
											LEFT OUTER JOIN
												EvacueeRegistrations ir
												ON r.Id = ir.Id
											WHERE
												ir.Id IS NULL");

            migrationBuilder.Sql(@"WITH HeadOfHousehold
                                                AS
                                                (
	                                                SELECT reg.Id as RegistrationId, p.Id as HohId
	                                                FROM
		                                                Registrations reg
	                                                INNER JOIN
		                                                People p
		                                                ON reg.HeadOfHouseholdId = p.Id
                                                ),
                                                RegToEvacuee
                                                AS
                                                (
	                                                SELECT DISTINCT hoh.RegistrationId, p.Id AS PeopleId
	                                                FROM
		                                                People p
	                                                LEFT OUTER JOIN
		                                                HeadOfHousehold hoh
		                                                ON p.id = hoh.HohId OR p.HeadOfHouseholdId = hoh.HohId
	                                                LEFT OUTER JOIN
		                                                People fam
		                                                ON p.Id = fam.HeadOfHouseholdId
                                                )
                                                INSERT INTO [dbo].[Evacuees]
                                                           ([EvacueeRegistrationId]
                                                           ,[EvacueeSequenceNumber]
                                                           ,[FirstName]
                                                           ,[LastName]
                                                           ,[Nickname]
                                                           ,[Initials]
                                                           ,[SameLastNameAsEvacuee]
                                                           ,[Gender]
                                                           ,[Dob]
                                                           ,[BcServicesNumber]
                                                           ,[EvacueeTypeCode])
                                                SELECT
                                                           reg.RegistrationId,
                                                           ROW_NUMBER() OVER(PARTITION BY reg.RegistrationId ORDER BY reg.RegistrationId),
                                                           p.FirstName,
                                                           p.LastName,
                                                           p.Nickname,
                                                           p.Initials,
                                                           ISNULL(p.SameLastNameAsEvacuee, 0),
                                                           p.Gender,
                                                           p.Dob,
                                                           p.BcServicesNumber,
                                                           ISNULL(p.RelationshipToEvacueeCode, 'HOH')
                                                FROM
	                                                People p
                                                INNER JOIN
	                                                RegToEvacuee reg
	                                                ON p.Id = reg.PeopleId
                                                ");

            migrationBuilder.Sql(@"WITH newAddresses AS
                                                (
	                                                SELECT
		                                                r.EssFileNumber as RegistrationId,
														1 AS AddressSequence,
		                                                p.PrimaryResidenceId,
		                                                'Primary' AS AddressType,
		                                                addr.AddressSubtype,
		                                                addr.AddressLine1,
		                                                addr.AddressLine2,
		                                                addr.AddressLine3,
		                                                addr.PostalCode,
		                                                addr.Province,
		                                                addr.CommunityId,
		                                                addr.City,
		                                                addr.CountryCode
	                                                FROM
		                                                People p
	                                                INNER JOIN
		                                                Addresses addr
		                                                ON p.PrimaryResidenceId = addr.Id
	                                                INNER JOIN
		                                                Registrations r
														ON p.Id = r.HeadOfHouseholdId
	                                                UNION
	                                                SELECT
		                                                r.EssFileNumber as RegistrationId,
														2 AS AddressSequence,
		                                                p.PrimaryResidenceId,
		                                                'Mailing' AS AddressType,
		                                                addr.AddressSubtype,
		                                                addr.AddressLine1,
		                                                addr.AddressLine2,
		                                                addr.AddressLine3,
		                                                addr.PostalCode,
		                                                addr.Province,
		                                                addr.CommunityId,
		                                                addr.City,
		                                                addr.CountryCode
	                                                FROM
		                                                People p
	                                                INNER JOIN
		                                                Addresses addr
		                                                ON p.MailingAddressId = addr.Id
	                                                INNER JOIN
		                                                Registrations r
														ON p.Id = r.HeadOfHouseholdId
                                                )
												INSERT INTO [dbo].[EvacueeRegistrationAddresses]
														   ([RegistrationId]
														   ,[AddressSequenceNumber]
														   ,[AddressTypeCode]
														   ,[AddressSubtypeCode]
														   ,[AddressLine1]
														   ,[AddressLine2]
														   ,[AddressLine3]
														   ,[PostalCode]
														   ,[CommunityId]
														   ,[City]
														   ,[Province]
														   ,[CountryCode])
                                                SELECT DISTINCT
	                                                a.RegistrationId,
                                                    a.AddressSequence,
	                                                a.AddressType,
	                                                a.AddressSubtype,
	                                                a.AddressLine1,
	                                                a.AddressLine2,
	                                                a.AddressLine3,
	                                                a.PostalCode,
	                                                a.CommunityId,
	                                                a.City,
	                                                a.Province,
													a.CountryCode
                                                FROM
	                                                newAddresses a
                                                ");

            migrationBuilder.CreateIndex(
                name: "IX_EvacueeRegistrationAddresses_CommunityId",
                table: "EvacueeRegistrationAddresses",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_EvacueeRegistrationAddresses_CountryCode",
                table: "EvacueeRegistrationAddresses",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_EvacueeRegistrations_HostCommunityId",
                table: "EvacueeRegistrations",
                column: "HostCommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_EvacueeRegistrations_IncidentTaskId",
                table: "EvacueeRegistrations",
                column: "IncidentTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvacueeRegistrationAddresses");

            migrationBuilder.DropTable(
                name: "Evacuees");

            migrationBuilder.DropTable(
                name: "EvacueeRegistrations");
        }
    }
}