using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class Organization
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public bool Active { get; set; }

        [MaxLength(150)]
        [Required]
        public string Name { get; set; }

        [MaxLength(100)]
        [Column("BceidAccountNumber")]
        public string BCeIDBusinessGuid { get; set; }

        [ForeignKey("Region")]
        public string RegionName { get; set; }

        public Guid? CommunityId { get; set; }

        public Region Region { get; set; }

        public Community Community { get; set; }
    }
}
