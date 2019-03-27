using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Role Database Model
    /// </summary>
    public class Region
    {
        public Region()
        { }

        /// <summary>
        /// A system-generated unique identifier
        /// </summary>
        /// <value>A system-generated unique identifier for a Role</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the Region
        /// </summary>
        /// <value>The name of the Region</value>
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// true if active
        /// </summary>
        public bool? Active { get; set; }

        public virtual IEnumerable<Organization> Organizations { get; set; }
    }
}
