using Gov.Jag.Embc.Public.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Gov.Jag.Embc.Public.Models.Db.Enumerations;

namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Person Database Model
    /// </summary>
    public class Evacuee
    {
        [Key, Column(Order = 0)]
        public Guid IncidentRegistrationId { get; set; }

        [Key, Column(Order = 1)]
        public int EvacueeSequenceNumber { get; set; }

        public string IncidentRegSeqId => $"{IncidentRegistration}-{EvacueeSequenceNumber}";

        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string Nickname { get; set; }

        [MaxLength(255)]
        public string Initials { get; set; }

        public bool SameLastNameAsEvacuee { get; set; }

        [MaxLength(255)]
        public string Gender { get; set; }

        public DateTime? Dob { get; set; }

        public string BcServicesNumber { get; set; }

        public string EvacueeTypeCode { get; set; }

        public EvacueeType EvacueeType => string.IsNullOrEmpty(EvacueeTypeCode) ? EvacueeType.NotSet : EnumHelper<EvacueeType>.GetValueFromName(EvacueeTypeCode);

        public IncidentRegistration IncidentRegistration { get; set; }

        public static Guid GetIncidentRegistrationIdFromIncidentRegSeqId(string incidentRegSeqId)
        {
            return Guid.Parse(incidentRegSeqId.Split(',')[0]);
        }

        public static int GetEvacueeSequenceNumberFromIncidentRegSeqId(string incidentRegSeqId)
        {
            return Convert.ToInt32(incidentRegSeqId.Split(',')[1]);
        }
    }
}
