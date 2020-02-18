using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class IncidentTask : IAuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public string TaskNumber { get; set; }
        public string Details { get; set; }

        public bool Active { get; set; }

        [ForeignKey("Region")]
        public string RegionName { get; set; }

        public Region Region { get; set; }

        public Guid? CommunityId { get; set; }
        // Host Community

        public Community Community { get; set; }

        public IEnumerable<EvacueeRegistration> EvacueeRegistrations { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? TaskNumberStartDate { get; set; }
        public DateTimeOffset? TaskNumberEndDate { get; set; }
    }
}
