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
                .Without(f => f.PersonType)
                .Without(f => f.Id));

            fixture.Customize<HeadOfHousehold>(c => c
                .Without(f => f.PersonType)
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
            registration.Id = regId;

            var result = await di.GetEvacueeRegistrationAsync(regId);

            AssertRegistration(registration, result, generateCompleteRegistration);
        }

        private static void AssertRegistration(Registration original, Registration result, bool isComplete)
        {
            Assert.NotNull(result);

            Assert.Equal(original.Id, result.Id);
            Assert.Equal(isComplete, result.IsFinalized);
            Assert.Equal(original.CompletedBy, result.CompletedBy);
            Assert.Equal(original.HostCommunity?.Id, result.HostCommunity?.Id);
            Assert.Equal(original.IncidentTask?.Id, result.IncidentTask?.Id);
            Assert.Equal(original.DeclarationAndConsent, result.DeclarationAndConsent);
            Assert.Equal(original.DietaryNeeds, result.DietaryNeeds);
            Assert.Equal(original.DietaryNeedsDetails, result.DietaryNeedsDetails);
            Assert.Equal(original.DisasterAffectDetails, result.DisasterAffectDetails);
            Assert.Equal(original.ExternalReferralsDetails, result.ExternalReferralsDetails);
            Assert.Equal(original.Facility, result.Facility);
            Assert.Equal(original.FamilyRecoveryPlan, result.FamilyRecoveryPlan);
            Assert.Equal(original.HasChildCareReferral, result.HasChildCareReferral);
            Assert.Equal(original.HasFirstAidReferral, result.HasFirstAidReferral);
            Assert.Equal(original.HasHealthServicesReferral, result.HasHealthServicesReferral);
            Assert.Equal(original.HasInquiryReferral, result.HasInquiryReferral);
            Assert.Equal(original.HasPersonalServicesReferral, result.HasPersonalServicesReferral);
            Assert.Equal(original.HasPetCareReferral, result.HasPetCareReferral);
            Assert.Equal(original.HasPets, result.HasPets);
            Assert.Equal(original.HasThreeDayMedicationSupply, result.HasThreeDayMedicationSupply);
            Assert.Equal(original.InsuranceCode, result.InsuranceCode);
            Assert.Equal(original.InternalCaseNotes, result.InternalCaseNotes);
            Assert.Equal(original.MedicationNeeds, result.MedicationNeeds);
            Assert.Equal(original.RegisteringFamilyMembers, result.RegisteringFamilyMembers);
            Assert.Equal(original.RegistrationCompletionDate, result.RegistrationCompletionDate);
            Assert.Equal(original.RequiresAccommodation, result.RequiresAccommodation);
            Assert.Equal(original.RequiresClothing, result.RequiresClothing);
            Assert.Equal(original.RequiresFood, result.RequiresFood);
            Assert.Equal(original.RequiresIncidentals, result.RequiresIncidentals);
            Assert.Equal(original.RequiresSupport, result.RequiresSupport);
            Assert.Equal(original.RequiresTransportation, result.RequiresTransportation);
            Assert.Equal(original.RestrictedAccess, result.RestrictedAccess);
            Assert.Equal(original.SelfRegisteredDate, result.SelfRegisteredDate);
            Assert.NotNull(result.HeadOfHousehold);
            Assert.NotNull(result.HeadOfHousehold.Id);
            Assert.Equal($"{original.Id}-1", result.HeadOfHousehold.Id);
            Assert.Equal(original.HeadOfHousehold.Dob, result.HeadOfHousehold.Dob);
            Assert.Equal(original.HeadOfHousehold.Email, result.HeadOfHousehold.Email);
            Assert.Equal(original.HeadOfHousehold.FirstName, result.HeadOfHousehold.FirstName);
            Assert.Equal(original.HeadOfHousehold.Gender, result.HeadOfHousehold.Gender);
            Assert.Equal(original.HeadOfHousehold.Initials, result.HeadOfHousehold.Initials);
            Assert.Equal(original.HeadOfHousehold.LastName, result.HeadOfHousehold.LastName);
            Assert.Equal(original.HeadOfHousehold.Nickname, result.HeadOfHousehold.Nickname);
            Assert.Equal(original.HeadOfHousehold.PhoneNumber, result.HeadOfHousehold.PhoneNumber);
            Assert.Equal(original.HeadOfHousehold.PhoneNumberAlt, result.HeadOfHousehold.PhoneNumberAlt);
            Assert.Equal($"{original.Id}-2", result.HeadOfHousehold.MailingAddress?.Id);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.AddressLine1, result.HeadOfHousehold.MailingAddress?.AddressLine1);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.AddressLine2, result.HeadOfHousehold.MailingAddress?.AddressLine2);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.AddressLine3, result.HeadOfHousehold.MailingAddress?.AddressLine3);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.AddressSubtype, result.HeadOfHousehold.MailingAddress?.AddressSubtype);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.City, result.HeadOfHousehold.MailingAddress?.City);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.Community?.Id, result.HeadOfHousehold.MailingAddress?.Community?.Id);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.Country?.Id, result.HeadOfHousehold.MailingAddress?.Country?.Id);
            Assert.NotNull(result.HeadOfHousehold.PrimaryResidence);
            Assert.NotNull(result.HeadOfHousehold.PrimaryResidence.Id);
            Assert.Equal($"{original.Id}-1", result.HeadOfHousehold.PrimaryResidence.Id);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.AddressLine1, result.HeadOfHousehold.PrimaryResidence.AddressLine1);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.AddressLine2, result.HeadOfHousehold.PrimaryResidence.AddressLine2);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.AddressLine3, result.HeadOfHousehold.PrimaryResidence.AddressLine3);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.AddressSubtype, result.HeadOfHousehold.PrimaryResidence.AddressSubtype);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.City, result.HeadOfHousehold.PrimaryResidence.City);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.Community?.Id, result.HeadOfHousehold.PrimaryResidence.Community?.Id);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.Country?.Id, result.HeadOfHousehold.PrimaryResidence.Country?.Id);
            Assert.Equal(original.HeadOfHousehold.FamilyMembers.Count(), result.HeadOfHousehold.FamilyMembers.Count());
            Assert.All(original.HeadOfHousehold.FamilyMembers, fmbr =>
            {
                var resultFmbr = result.HeadOfHousehold.FamilyMembers.Single(f => f.FirstName == fmbr.FirstName && f.Dob == fmbr.Dob);
                Assert.NotNull(resultFmbr.Id);
                Assert.StartsWith(original.Id, resultFmbr.Id);
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

        [Theory]
        [RegistrationInlineAutoData(false)]
        [RegistrationInlineAutoData(true)]
        public async Task GetRegistrationSummary_Registration_Retrieved(bool generateCompleteRegistration, Registration registration)
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
            registration.Id = regId;

            var result = await di.GetEvacueeRegistrationSummaryAsync(regId);

            AssertRegistrationSummary(registration, result);
        }

        private static void AssertRegistrationSummary(Registration original, RegistrationSummary result)
        {
            Assert.NotNull(result);

            Assert.Equal(original.Id, result.Id);
            Assert.Equal(!string.IsNullOrEmpty(original.InternalCaseNotes), result.HasInternalCaseNotes);
            Assert.Equal(original.HostCommunity?.Id, result.HostCommunity?.Id);
            Assert.Equal(original.IncidentTask?.Id, result.IncidentTask?.Id);
            Assert.Equal(original.Facility, result.Facility);
            Assert.Equal(original.RegisteringFamilyMembers, result.RegisteringFamilyMembers);
            Assert.Equal(original.RegistrationCompletionDate, result.RegistrationCompletionDate);
            Assert.Equal(original.RequiresAccommodation, result.RequiresAccommodation);
            Assert.Equal(original.RequiresClothing, result.RequiresClothing);
            Assert.Equal(original.RequiresFood, result.RequiresFood);
            Assert.Equal(original.RequiresIncidentals, result.RequiresIncidentals);
            Assert.Equal(original.RequiresSupport, result.RequiresSupport);
            Assert.Equal(original.RequiresTransportation, result.RequiresTransportation);
            Assert.Equal(original.RestrictedAccess, result.RestrictedAccess);
            Assert.Equal(original.SelfRegisteredDate, result.SelfRegisteredDate);
            Assert.NotNull(result.HeadOfHousehold);
            Assert.NotNull(result.HeadOfHousehold.Id);
            Assert.Equal($"{original.Id}-1", result.HeadOfHousehold.Id);
            Assert.Equal(original.HeadOfHousehold.Dob, result.HeadOfHousehold.Dob);
            Assert.Equal(original.HeadOfHousehold.Email, result.HeadOfHousehold.Email);
            Assert.Equal(original.HeadOfHousehold.FirstName, result.HeadOfHousehold.FirstName);
            Assert.Equal(original.HeadOfHousehold.Gender, result.HeadOfHousehold.Gender);
            Assert.Equal(original.HeadOfHousehold.Initials, result.HeadOfHousehold.Initials);
            Assert.Equal(original.HeadOfHousehold.LastName, result.HeadOfHousehold.LastName);
            Assert.Equal(original.HeadOfHousehold.Nickname, result.HeadOfHousehold.Nickname);
            Assert.Equal(original.HeadOfHousehold.PhoneNumber, result.HeadOfHousehold.PhoneNumber);
            Assert.Equal(original.HeadOfHousehold.PhoneNumberAlt, result.HeadOfHousehold.PhoneNumberAlt);
            Assert.Equal($"{original.Id}-2", result.HeadOfHousehold.MailingAddress?.Id);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.AddressLine1, result.HeadOfHousehold.MailingAddress?.AddressLine1);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.AddressLine2, result.HeadOfHousehold.MailingAddress?.AddressLine2);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.AddressLine3, result.HeadOfHousehold.MailingAddress?.AddressLine3);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.AddressSubtype, result.HeadOfHousehold.MailingAddress?.AddressSubtype);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.City, result.HeadOfHousehold.MailingAddress?.City);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.Community?.Id, result.HeadOfHousehold.MailingAddress?.Community?.Id);
            Assert.Equal(original.HeadOfHousehold.MailingAddress?.Country?.Id, result.HeadOfHousehold.MailingAddress?.Country?.Id);
            Assert.NotNull(result.HeadOfHousehold.PrimaryResidence);
            Assert.NotNull(result.HeadOfHousehold.PrimaryResidence.Id);
            Assert.Equal($"{original.Id}-1", result.HeadOfHousehold.PrimaryResidence.Id);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.AddressLine1, result.HeadOfHousehold.PrimaryResidence.AddressLine1);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.AddressLine2, result.HeadOfHousehold.PrimaryResidence.AddressLine2);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.AddressLine3, result.HeadOfHousehold.PrimaryResidence.AddressLine3);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.AddressSubtype, result.HeadOfHousehold.PrimaryResidence.AddressSubtype);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.City, result.HeadOfHousehold.PrimaryResidence.City);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.Community?.Id, result.HeadOfHousehold.PrimaryResidence.Community?.Id);
            Assert.Equal(original.HeadOfHousehold.PrimaryResidence.Country?.Id, result.HeadOfHousehold.PrimaryResidence.Country?.Id);
            Assert.Equal(original.HeadOfHousehold.FamilyMembers.Count(), result.HeadOfHousehold.FamilyMembers.Count());
            Assert.All(original.HeadOfHousehold.FamilyMembers, fmbr =>
            {
                var resultFmbr = result.HeadOfHousehold.FamilyMembers.Single(f => f.FirstName == fmbr.FirstName && f.Dob == fmbr.Dob);
                Assert.NotNull(resultFmbr.Id);
                Assert.StartsWith(original.Id, resultFmbr.Id);
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