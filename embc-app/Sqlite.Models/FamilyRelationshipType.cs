using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Sqlite.Models
{
    /// <summary>
    /// Database Model
    /// </summary>
    public sealed partial class FamilyRelationshipType
    {
        /// <summary>
        /// A system-generated unique identifier
        /// </summary>
        /// <value>The short character string, such as an acronym or abbreviation, which identifies the instance.</value>

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string Code { get; set; }

        /// <summary>
        /// The word or phrase that identifies the instance. e.g. "Immediate Family"
        /// </summary>
        [MaxLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// true if active
        /// </summary>
        public bool? Active { get; set; }

        public FamilyRelationshipType()
        { }
    }
}
