using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Incident Task Database Model
    /// </summary>
    public class IncidentTask
    {
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

        public DateTimeOffset? StartDate { get; set; }
    }
}
