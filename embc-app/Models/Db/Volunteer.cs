using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Volunteer Database Model
    /// </summary>
    public class Volunteer : IAuditableEntity
    {
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

        public string BceidAccountUserName { get; set; }
        public bool? IsNewUser { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsPrimaryContact { get; set; }
        public bool? CanAccessRestrictedFiles { get; set; }

        [MaxLength(255)]
        [Column("UserId")]
        public string BCeId { get; set; }

        public Guid? OrganizationId { get; set; }

        public Organization Organization { get; set; }
    }
}
