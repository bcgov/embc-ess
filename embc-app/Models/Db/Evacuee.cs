using Gov.Jag.Embc.Public.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Gov.Jag.Embc.Public.Models.Db.Enumerations;

namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Person Database Model
    /// </summary>
    public class Evacuee : IAuditableEntity
    {
        public long RegistrationId { get; set; }

        public int EvacueeSequenceNumber { get; set; }

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

        public EvacueeType EvacueeType => string.IsNullOrEmpty(EvacueeTypeCode) ? EvacueeType.NotSet : EnumHelper<EvacueeType>.GetValueFromDisplayName(EvacueeTypeCode);

        public EvacueeRegistration EvacueeRegistration { get; set; }

        public IEnumerable<ReferralEvacuee> Referrals { get; set; }
    }
}
