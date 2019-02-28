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
    /// Person Database Model
    /// </summary>
    public sealed partial class Profile
    {

        public Profile()
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
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        public string PhoneNumberAlt { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string Initials { get; set; }

        [MaxLength(255)]
        public Address PrimaryResidence { get; set; }
        
        public Address MailingAddress { get; set; }

  
        
    }
}
