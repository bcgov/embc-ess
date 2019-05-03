using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class RegistrationAddress
    {

        [Key, Column(Order = 0)]
        public Guid IncidentRegistrationId { get; set; }

        [Key, Column(Order = 1)]
        public int AddressSequenceNumber { get; set; }

        public string AddressSubtype { get; set; }  // one of ['BCAD', 'OTAD'] for BC vs non-BC addresses

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string PostalCode { get; set; }

        public Guid CommunityId { get; set; }

        public Community Community { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        [ForeignKey("Country")]
        public string CountryCode { get; set; }

        public Country Country { get; set; }

        public IncidentRegistration IncidentRegistration { get; set; }
    }
}
