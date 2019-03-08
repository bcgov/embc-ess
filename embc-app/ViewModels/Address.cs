using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.ViewModels
{    

    public class Address
    {
        public string Id { get; set; }

        [MaxLength(255)]
        public string AddressLine1 { get; set; }

        [MaxLength(255)]
        public string AddressLine2 { get; set; }

        [MaxLength(255)]
        public string AddressLine3 { get; set; }

        [MaxLength(255)]
        public string CommunityOrCity { get; set; }

        [MaxLength(255)]
        public string Province { get; set; }

        [MaxLength(255)]
        public string PostalCode { get; set; }

        [MaxLength(255)]
        public string Country { get; set; }
    }
}
