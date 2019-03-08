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
    /// Address Database Model
    /// </summary>
        public sealed partial class Address
    {

        public Address()
        { }

        /// <summary>
        /// A system-generated unique identifier for a Role
        /// </summary>
        /// <value>A system-generated unique identifier for a Role</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [MaxLength(255)]        
        public string AddressLine1 { get; set; }    

        [MaxLength(255)]
        public string AddressLine2 { get; set; }

        [MaxLength(255)]
        public string AddressLine3 { get; set; }

        [MaxLength(255)]
        public string CommunityOrCity { get; set; }

        [MaxLength(255)]
        public string Province { get; set; }

        [MaxLength(255)]
        public string PostalCode { get; set; }

        [MaxLength(255)]
        public string Country { get; set; }
        
    }
}
