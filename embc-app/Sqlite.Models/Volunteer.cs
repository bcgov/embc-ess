using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.Sqlite.Models
{
    /// <summary>
    /// Volunteer Database Model
    /// </summary>
    public sealed partial class Volunteer : Person
    {
        [MaxLength(255)]
        public string Name { get; set; } // TODO: What's this? Their display name? full name?

        [MaxLength(255)]
        public string Email { get; set; }

        public string BceidAccountNumber { get; set; }
        public bool? IsNewUser { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsPrimaryContact { get; set; }
        public bool? CanAccessRestrictedFiles { get; set; }

        // siteminder guid
        public string Externaluseridentifier { get; set; }

        // related entities
        public Organization Organization { get; set; }


        public Volunteer()
        {
            PersonType = Person.VOLUNTEER;
        }
    }
}
