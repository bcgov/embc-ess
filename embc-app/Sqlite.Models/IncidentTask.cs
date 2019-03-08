using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Sqlite.Models
{
    /// <summary>
    /// Incident Task Database Model
    /// </summary>
    public sealed partial class IncidentTask
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
        public Region Region { get; set; }
        public RegionalDistrict RegionalDistrict { get; set; }
        public Community Community { get; set; }

    }
}
