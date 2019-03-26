using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class Evacuee : Person
    {
        public string BcServicesNumber { get; set; }
    }

    public class HeadOfHousehold : Evacuee
    {
        // evacuee information (HOH and family members)
        public string PhoneNumber { get; set; }

        public string PhoneNumberAlt { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // related entities
        public Address PrimaryResidence { get; set; }

        public Address MailingAddress { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; }

        public HeadOfHousehold()
        {
            PersonType = Models.Db.Person.HOH;
        }
    }
}
