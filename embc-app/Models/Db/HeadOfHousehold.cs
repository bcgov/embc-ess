using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public abstract class Evacuee : Person
    {
        public string BcServicesNumber { get; set; }
    }

    public class HeadOfHousehold : Evacuee
    {
        // evacuee information (HOH and family members)
        public string PhoneNumber { get; set; }

        public string PhoneNumberAlt { get; set; }
        public string Email { get; set; }

        // related entities
        public virtual Address PrimaryResidence { get; set; }

        public virtual Address MailingAddress { get; set; }
        public virtual List<FamilyMember> FamilyMembers { get; set; }

        public HeadOfHousehold()
        {
            PersonType = Person.HOH;
        }
    }
}
