using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Volunteer Database Model
    /// </summary>
    public class VolunteerTask : IAuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; }

        public Guid IncidentTaskId { get; set; }
        public IncidentTask IncidentTask { get; set; }

    }
}
