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
    public partial class Person
    {

        public Person()
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
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string Nickname { get; set; }

        [MaxLength(255)]
        public string Initials { get; set; }

        [MaxLength(255)]
        public string Gender { get; set; }
        
        public long Dob { get; set; }

        // --> contact details (phone, email, address, etc)
        //profile: Profile;

  // TODO: ????

        public bool? IsEvacuee { get; set; }
        public bool IsVolunteer { get; set; }
        public bool IsFamilyMember { get; set; }

    }
}
