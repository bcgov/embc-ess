using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class Community
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public bool Active { get; set; }
        [ForeignKey("Region")]
        [Required]
        public string RegionName { get; set; }
        public Region Region { get; set; }
        public IEnumerable<Organization> Organizations { get; set; }
    }
}
