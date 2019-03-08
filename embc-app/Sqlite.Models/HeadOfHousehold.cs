using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Sqlite.Models
{   

    public class Evacuee : Person
    {
        public string BcServicesNumber { get; set; }
    }

    public sealed class HeadOfHousehold : Evacuee
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
