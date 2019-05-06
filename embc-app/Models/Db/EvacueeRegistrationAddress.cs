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
    public class EvacueeRegistrationAddress : IAuditableEntity
    {

        [Key, Column(Order = 0)]
        public Guid EvacueeRegistrationId { get; set; }

        [Key, Column(Order = 1)]
        public int AddressSequenceNumber { get; set; }

        public string EvacueeRegSeqId => $"{EvacueeRegistrationId}-{AddressSequenceNumber}";

        public string AddressTypeCode { get; set; }

        public AddressType AddressType => string.IsNullOrEmpty(AddressTypeCode) ?
            AddressType.NotSet : EnumHelper<AddressType>.GetValueFromDisplayName(AddressTypeCode);

        public string AddressSubtypeCode { get; set; }

        public AddressSubType AddressSubType => string.IsNullOrEmpty(AddressSubtypeCode)
            ? AddressSubType.NotSet : EnumHelper<AddressSubType>.GetValueFromDisplayName(AddressSubtypeCode);

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

        public EvacueeRegistration EvacueeRegistration { get; set; }

        public static Guid GetEvacueeRegistrationIdFromIncidentRegSeqId(string incidentRegSeqId)
        {
            return Guid.Parse(incidentRegSeqId.Split(',')[0]);
        }

        public static int GetAddressSequenceNumberFromIncidentRegSeqId(string incidentRegSeqId)
        {
            return Convert.ToInt32(incidentRegSeqId.Split(',')[1]);
        }
    }
}
