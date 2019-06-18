using AutoFixture;
using AutoFixture.Xunit2;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests.Registrations
{
    public class RegistrationAutoDataAttribute : AutoDataAttribute
    {
        public RegistrationAutoDataAttribute() : base(() => new Fixture()
        .Customize(new RegistrationCustomization()))
        { }
    }

    public class RegistrationInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public RegistrationInlineAutoDataAttribute(params object[] values) : base(new RegistrationAutoDataAttribute(), values)
        {
        }
    }

    public class RegistrationCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Registration>(c => c
                .With(r => r.Active, true)
                .Without(r => r.Id)
                .Without(r => r.HostCommunity)
                .Without(r => r.IncidentTask)
                .Without(r => r.CompletedBy)
                .Without(r => r.RegistrationCompletionDate));

            fixture.Customizations.Add(
                new ElementsBuilder<FamilyRelationshipType>(
                    new FamilyRelationshipType { Code = "IMMF", Active = true, Description = "" },
                    new FamilyRelationshipType { Code = "EXTF", Active = true, Description = "" }
            ));

            fixture.Customize<FamilyMember>(c => c
               .Without(f => f.Id));

            fixture.Customize<HeadOfHousehold>(c => c
                .Without(h => h.Id)
                .With(h => h.MailingAddress, fixture.Build<Address>()
                    .Without(a => a.Id)
                    .With(a => a.AddressSubtype, "BCAD")
                    .Without(a => a.City)
                    .Without(a => a.Community)
                    .With(a => a.Country, new Country { Id = "CAN", CountryCode = "CAN", Name = "Canada" })
                    .Create())
                .With(h => h.PrimaryResidence, fixture.Build<Address>()
                    .Without(a => a.Id)
                    .With(a => a.AddressSubtype, "OTAD")
                    .Without(a => a.Community)
                    .With(a => a.Country, new Country { Id = "USA", CountryCode = "USA", Name = "USALAND" })
                    .Create())
                .With(h => h.FamilyMembers, fixture.CreateMany<FamilyMember>(5).ToList()));
        }
    }

    public class RegistrationsTests : TestBase
    {
        public RegistrationsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [RegistrationInlineAutoData(false)]
        [RegistrationInlineAutoData(true)]
        public async Task Create_Registration_Saved(bool generateCompleteRegistration, Registration registration)
        {
            var di = Services.ServiceProvider.GetService<IDataInterface>();

            if (generateCompleteRegistration)
            {
                var fromCommunity = await GetRandomSeededCommunity();
                var toCommunity = await GetRandomSeededCommunity();
                var task = IncidentTaskGenerator.Generate();
                task.Community = fromCommunity;
                var taskId = (await di.CreateIncidentTaskAsync(task)).Id;

                registration.HostCommunity = toCommunity;
                registration.IncidentTask = new IncidentTask { Id = taskId };
                registration.RegistrationCompletionDate = DateTime.Now;
            }

            //Workaround the auto generated community id issue
            registration.HeadOfHousehold.MailingAddress.Community = await GetRandomSeededCommunity();

            var regId = await di.CreateEvacueeRegistrationAsync(registration);
            var result = await di.GetEvacueeRegistrationAsync(regId);

            Assert.NotNull(result);

            Assert.Equal(generateCompleteRegistration, result.IsFinalized);
            Assert.Equal(registration.CompletedBy, result.CompletedBy);
            Assert.Equal(registration.HostCommunity?.Id, result.HostCommunity?.Id);
            Assert.Equal(registration.IncidentTask?.Id, result.IncidentTask?.Id);
            Assert.Equal(registration.DeclarationAndConsent, result.DeclarationAndConsent);
            Assert.Equal(registration.DietaryNeeds, result.DietaryNeeds);
            Assert.Equal(registration.DietaryNeedsDetails, result.DietaryNeedsDetails);
            Assert.Equal(registration.DisasterAffectDetails, result.DisasterAffectDetails);
            Assert.Equal(registration.ExternalReferralsDetails, result.ExternalReferralsDetails);
            Assert.Equal(registration.Facility, result.Facility);
            Assert.Equal(registration.FamilyRecoveryPlan, result.FamilyRecoveryPlan);
            Assert.Equal(registration.HasChildCareReferral, result.HasChildCareReferral);
            Assert.Equal(registration.HasFirstAidReferral, result.HasFirstAidReferral);
            Assert.Equal(registration.HasHealthServicesReferral, result.HasHealthServicesReferral);
            Assert.Equal(registration.HasInquiryReferral, result.HasInquiryReferral);
            Assert.Equal(registration.HasPersonalServicesReferral, result.HasPersonalServicesReferral);
            Assert.Equal(registration.HasPetCareReferral, result.HasPetCareReferral);
            Assert.Equal(registration.HasPets, result.HasPets);
            Assert.Equal(registration.HasThreeDayMedicationSupply, result.HasThreeDayMedicationSupply);
            Assert.Equal(registration.InsuranceCode, result.InsuranceCode);
            Assert.Equal(registration.InternalCaseNotes, result.InternalCaseNotes);
            Assert.Equal(registration.MedicationNeeds, result.MedicationNeeds);
            Assert.Equal(registration.RegisteringFamilyMembers, result.RegisteringFamilyMembers);
            Assert.Equal(registration.RegistrationCompletionDate, result.RegistrationCompletionDate);
            Assert.Equal(registration.RequiresAccommodation, result.RequiresAccommodation);
            Assert.Equal(registration.RequiresClothing, result.RequiresClothing);
            Assert.Equal(registration.RequiresFood, result.RequiresFood);
            Assert.Equal(registration.RequiresIncidentals, result.RequiresIncidentals);
            Assert.Equal(registration.RequiresSupport, result.RequiresSupport);
            Assert.Equal(registration.RequiresTransportation, result.RequiresTransportation);
            Assert.Equal(registration.RestrictedAccess, result.RestrictedAccess);
            Assert.Equal(registration.SelfRegisteredDate, result.SelfRegisteredDate);
            Assert.Equal(registration.HeadOfHousehold.Dob, result.HeadOfHousehold.Dob);
            Assert.Equal(registration.HeadOfHousehold.Email, result.HeadOfHousehold.Email);
            Assert.Equal(registration.HeadOfHousehold.FirstName, result.HeadOfHousehold.FirstName);
            Assert.Equal(registration.HeadOfHousehold.Gender, result.HeadOfHousehold.Gender);
            Assert.Equal(registration.HeadOfHousehold.Initials, result.HeadOfHousehold.Initials);
            Assert.Equal(registration.HeadOfHousehold.LastName, result.HeadOfHousehold.LastName);
            Assert.Equal(registration.HeadOfHousehold.Nickname, result.HeadOfHousehold.Nickname);
            Assert.Equal(registration.HeadOfHousehold.PhoneNumber, result.HeadOfHousehold.PhoneNumber);
            Assert.Equal(registration.HeadOfHousehold.PhoneNumberAlt, result.HeadOfHousehold.PhoneNumberAlt);
            Assert.Equal(registration.HeadOfHousehold.MailingAddress?.AddressLine1, result.HeadOfHousehold.MailingAddress?.AddressLine1);
            Assert.Equal(registration.HeadOfHousehold.MailingAddress?.AddressLine2, result.HeadOfHousehold.MailingAddress?.AddressLine2);
            Assert.Equal(registration.HeadOfHousehold.MailingAddress?.AddressLine3, result.HeadOfHousehold.MailingAddress?.AddressLine3);
            Assert.Equal(registration.HeadOfHousehold.MailingAddress?.AddressSubtype, result.HeadOfHousehold.MailingAddress?.AddressSubtype);
            Assert.Equal(registration.HeadOfHousehold.MailingAddress?.City, result.HeadOfHousehold.MailingAddress?.City);
            Assert.Equal(registration.HeadOfHousehold.MailingAddress?.Community?.Id, result.HeadOfHousehold.MailingAddress?.Community?.Id);
            Assert.Equal(registration.HeadOfHousehold.MailingAddress?.Country?.Id, result.HeadOfHousehold.MailingAddress?.Country?.Id);
            Assert.Equal(registration.HeadOfHousehold.PrimaryResidence.AddressLine1, result.HeadOfHousehold.PrimaryResidence.AddressLine1);
            Assert.Equal(registration.HeadOfHousehold.PrimaryResidence.AddressLine2, result.HeadOfHousehold.PrimaryResidence.AddressLine2);
            Assert.Equal(registration.HeadOfHousehold.PrimaryResidence.AddressLine3, result.HeadOfHousehold.PrimaryResidence.AddressLine3);
            Assert.Equal(registration.HeadOfHousehold.PrimaryResidence.AddressSubtype, result.HeadOfHousehold.PrimaryResidence.AddressSubtype);
            Assert.Equal(registration.HeadOfHousehold.PrimaryResidence.City, result.HeadOfHousehold.PrimaryResidence.City);
            Assert.Equal(registration.HeadOfHousehold.PrimaryResidence.Community?.Id, result.HeadOfHousehold.PrimaryResidence.Community?.Id);
            Assert.Equal(registration.HeadOfHousehold.PrimaryResidence.Country?.Id, result.HeadOfHousehold.PrimaryResidence.Country?.Id);
            Assert.All(registration.HeadOfHousehold.FamilyMembers, fmbr =>
            {
                var resultFmbr = result.HeadOfHousehold.FamilyMembers.Single(f => f.FirstName == fmbr.FirstName && f.Dob == fmbr.Dob);
                Assert.Equal(fmbr.Dob, resultFmbr.Dob);
                Assert.Equal(fmbr.FirstName, resultFmbr.FirstName);
                Assert.Equal(fmbr.Gender, resultFmbr.Gender);
                Assert.Equal(fmbr.Initials, resultFmbr.Initials);
                Assert.Equal(fmbr.LastName, resultFmbr.LastName);
                Assert.Equal(fmbr.Nickname, resultFmbr.Nickname);
                Assert.Equal(fmbr.RelationshipToEvacuee?.Code, resultFmbr.RelationshipToEvacuee?.Code);
                Assert.Equal(fmbr.SameLastNameAsEvacuee, resultFmbr.SameLastNameAsEvacuee);
            });
        }
    }
}