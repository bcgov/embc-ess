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
    /// Role Database Model
    /// </summary>
    public sealed partial class RegionalDistrict
    {

        public RegionalDistrict()
        { }

        /// <summary>
        /// A system-generated unique identifier
        /// </summary>
        /// <value>A system-generated unique identifier for a Role</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the Community
        /// </summary>
        /// <value>The name of the Community</value>
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// true if active
        /// </summary>
        public bool? Active { get; set; }

        public Region Region { get; set; }
    }
}
