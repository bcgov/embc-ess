using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.ViewModels
{


    public class Evacuee : Person
    {
        public string BcServicesNumber { get; set; }
    }

    public class HeadOfHousehold : Evacuee
    {
        // evacuee information (HOH and family members)
        public string PersonType { get; set; } //: 'HOH';
        public string PhoneNumber { get; set; }
        public string PhoneNumberAlt { get; set; }
        public string Email { get; set; }

        // related entities
        public Address PrimaryResidence { get; set; }
        public Address MailingAddress { get; set; }
    }
}
