using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class HeadOfHousehold : Person
    {
        public string PhoneNumber { get; set; }

        public string PhoneNumberAlt { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public Address PrimaryResidence { get; set; }

        public Address MailingAddress { get; set; }
        public IEnumerable<FamilyMember> FamilyMembers { get; set; }

        public HeadOfHousehold()
        {
            PersonType = Person.HOH;
        }
    }
}
