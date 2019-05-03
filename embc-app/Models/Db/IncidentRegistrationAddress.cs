using Gov.Jag.Embc.Public.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gov.Jag.Embc.Public.Models.Db.Enumerations;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class IncidentRegistrationAddress
    {

        [Key, Column(Order = 0)]
        public Guid IncidentRegistrationId { get; set; }

        [Key, Column(Order = 1)]
        public int AddressSequenceNumber { get; set; }

        public string AddressTypeCode { get; set; }

        public AddressType AddressType => string.IsNullOrEmpty(AddressTypeCode) ?
            AddressType.NotSet : EnumHelper<AddressType>.GetValueFromName(AddressSubtypeCode);

        public string AddressSubtypeCode { get; set; }

        public AddressSubType AddressSubType => string.IsNullOrEmpty(AddressSubtypeCode)
            ? AddressSubType.NotSet : EnumHelper<AddressSubType>.GetValueFromName(AddressSubtypeCode);

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
