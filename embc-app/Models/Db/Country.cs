using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class Country
    {
        [Key]
        public string CountryCode { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
