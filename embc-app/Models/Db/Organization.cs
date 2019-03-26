using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Organization Database Model
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// A system-generated unique identifier
        /// </summary>
        /// <value>A system-generated unique identifier for a Role</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public bool? Active { get; set; }  // no deletions from DB this is a soft delete.

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string BceidAccountNumber { get; set; }

        // siteminder guid
        public string Externaluseridentifier { get; set; }

        public int? RegionId { get; set; }
        public int? RegionalDistrictId { get; set; }
        public int? CommunityId { get; set; }

        //Entities
        public virtual Region Region { get; set; }
        public virtual RegionalDistrict RegionalDistrict { get; set; }
        public virtual Community Community { get; set; }
    }
}
