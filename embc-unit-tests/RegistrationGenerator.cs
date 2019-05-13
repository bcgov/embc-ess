using Gov.Jag.Embc.Public.ViewModels;
using System.Collections.Generic;

namespace embc_unit_tests
{
    public static class RegistrationGenerator
    {
        public static Registration Generate()
        {
            return new Registration
            {
                HeadOfHousehold = new HeadOfHousehold
                {
                    FamilyMembers = new List<FamilyMember>()
                    {
                        new FamilyMember(){RelationshipToEvacuee = new FamilyRelationshipType{ Code="IMMF" } },
                        new FamilyMember(){RelationshipToEvacuee = new FamilyRelationshipType{ Code="IMMF" } }
                    },
                    PrimaryResidence = new Address
                    {
                        AddressSubtype = "OTAD",
                        Country = new Country { CountryCode = "USA" }
                    }
                },
            };
        }
    }
}