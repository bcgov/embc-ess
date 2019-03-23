using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Sqlite.Models
{
    /// <summary>
    /// Incident Task Database Model
    /// </summary>
    public class IncidentTask
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
        public bool? Active { get; set; }

        // only one of the following will be set; ie a regional incident vs a community one, etc
        public virtual Region Region { get; set; }

        public virtual RegionalDistrict RegionalDistrict { get; set; }
        public virtual Community Community { get; set; }
    }
}
