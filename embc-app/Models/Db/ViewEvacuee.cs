using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Models.Db
{
    [Table("ViewEvacuee")]
    public class ViewEvacuee
    {
        public string Id { get; set; }
        public bool RestrictedAccess { get; set; }
        public bool IsHeadOfHousehold { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Initials { get; set; }
        public string RegistrationId { get; set; }
        public string IncidentTaskNumber { get; set; }
        public string EvacuatedFrom { get; set; }
        public string EvacuatedTo { get; set; }
        public DateTime? RegistrationCompletionDate { get; set; }
        public bool IsFinalized { get; set; }
        public bool HasReferrals { get; set; }
        public bool Active { get; set; }
    }
}
