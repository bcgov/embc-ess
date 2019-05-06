using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Incident Task Database Model
    /// </summary>
    public class IncidentTask : IAuditableEntity
    {
        public IncidentTask()
        { }

        /// <summary>
        /// A system-generated unique identifier
        /// </summary>
        /// <value>A system-generated unique identifier for a Role</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public string TaskNumber { get; set; }
        public string Details { get; set; }

        /// <summary>
        /// true if active
        /// </summary>
        public bool Active { get; set; }

        // only one of the following will be set; ie a regional incident vs a community one, etc
        [ForeignKey("Region")]
        public string RegionName { get; set; }

        public Region Region { get; set; }

        public Guid? CommunityId { get; set; }

        public Community Community { get; set; }

        public List<Registration> Registrations { get; set; }

        public List<EvacueeRegistration> EvacueeRegistrations { get; set; }

        public DateTimeOffset? StartDate { get; set; }
    }
}
