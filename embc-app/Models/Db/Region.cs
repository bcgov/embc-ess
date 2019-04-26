using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class Region
    {
        [Key]
        [MaxLength(255)]
        public string Name { get; set; }

        public bool Active { get; set; }

        public IEnumerable<Organization> Organizations { get; set; }
    }
}
