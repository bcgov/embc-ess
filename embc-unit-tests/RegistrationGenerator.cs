using Gov.Jag.Embc.Public.ViewModels;
using System;
using System.Collections.Generic;

namespace embc_unit_tests
{
    public static class RegistrationGenerator
    {
        public static Registration GenerateSelf()
        {
            return new Registration
            {
                Active = true,
                DeclarationAndConsent = true,
                RestrictedAccess = false,
                HeadOfHousehold = new HeadOfHousehold
                {
                    FamilyMembers = new List<FamilyMember>()
                    {
                        new FamilyMember(){RelationshipToEvacuee = new FamilyRelationshipType{ Code="IMMF" }, FirstName= "fmbr1", LastName="evacuee", Nickname="IMMF",Initials="FMBR",Dob=DateTime.Parse("1980-01-10")},
                        new FamilyMember(){RelationshipToEvacuee = new FamilyRelationshipType{ Code="EXTF" }, FirstName= "fmbr2", LastName="evacuee", Nickname="EXTF",Initials="FMBR",Dob=DateTime.Parse("1910-12-31")},
                    },
                    PrimaryResidence = new Address
                    {
                        AddressSubtype = "OTAD",
                        Country = new Country { CountryCode = "USA" }
                    }
                },
            };
        }

        public static Registration GenerateCompleted(string incidentTaskId, string hostCommunityId)
        {
            var registration = GenerateSelf();
            registration.IncidentTask = new IncidentTask { Id = incidentTaskId };
            registration.HostCommunity = new Community { Id = hostCommunityId };
            registration.RegistrationCompletionDate = DateTime.Now;
            return registration;
        }
    }
}