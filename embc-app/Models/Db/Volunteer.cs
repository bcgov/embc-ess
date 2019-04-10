using System;
using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Volunteer Database Model
    /// </summary>
    public class Volunteer : Person
    {
        [MaxLength(255)]
        public string Email { get; set; }

        public bool Active { get; set; }

        public string BceidAccountNumber { get; set; }
        public bool? IsNewUser { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsPrimaryContact { get; set; }
        public bool? CanAccessRestrictedFiles { get; set; }

        // siteminder guid
        public string Externaluseridentifier { get; set; }

        // related entities
        public Guid? OrganizationId { get; set; }

        public Organization Organization { get; set; }

        public Volunteer()
        {
            PersonType = Person.VOLUNTEER;
        }
    }
}
