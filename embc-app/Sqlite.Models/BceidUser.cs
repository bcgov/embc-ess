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
        public sealed partial class Volunteer
    {

        public Volunteer()
        { }

        /// <summary>
        /// A system-generated unique identifier for a Role
        /// </summary>
        /// <value>A system-generated unique identifier for a Role</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [MaxLength(255)]        
        public string Name { get; set; }

        [MaxLength(255)]
        public string Firstname { get; set; }

        [MaxLength(255)]
        public string Lastname { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public bool? IsNewUser { get; set; }


    }
}
