using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public abstract class Evacuee : Person
    {
        [MaxLength(255)]
        public string Nickname { get; set; }

        [MaxLength(255)]
        public string Initials { get; set; }

        [MaxLength(255)]
        public string Gender { get; set; }

        public DateTimeOffset? Dob { get; set; }
        public string BcServicesNumber { get; set; }
    }

    public class HeadOfHousehold : Evacuee
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
