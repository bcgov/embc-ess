using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.Sqlite.Models
{
    public abstract class Evacuee : Person
    {
        public string BcServicesNumber { get; set; }
    }

    public sealed class HeadOfHousehold : Evacuee
    {
        // evacuee information (HOH and family members)
        public string PhoneNumber { get; set; }
        public string PhoneNumberAlt { get; set; }
        public string Email { get; set; }

        // related entities
        public Address PrimaryResidence { get; set; }
        public Address MailingAddress { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; }

        public HeadOfHousehold()
        {
            PersonType = Person.HOH;
        }
    }
}
