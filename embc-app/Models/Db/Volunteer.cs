using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Volunteer Database Model
    /// </summary>
    public class Volunteer
    {
        public Volunteer()
        {
            //CompletedRegistrations = new List<Registration>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public bool Active { get; set; }

        public string BceidAccountNumber { get; set; }
        public bool? IsNewUser { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsPrimaryContact { get; set; }
        public bool? CanAccessRestrictedFiles { get; set; }

        // siteminder guid
        public Guid? Externaluseridentifier { get; set; }

        // related entities
        public Guid? OrganizationId { get; set; }

        public Organization Organization { get; set; }
    }
}
