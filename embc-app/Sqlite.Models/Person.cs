using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Sqlite.Models
{
    /// <summary>
    /// Person Database Model
    /// </summary>
    public abstract partial class Person
    {
        public const string VOLUNTEER = "VOLN";
        public const string HOH = "HOH";
        public const string FAMILY_MEMBER = "FMBR";

        public static Person Create(string subType)
        {
            if (subType == VOLUNTEER)
            {
                return new Volunteer();
            }
            else if (subType == HOH)
            {
                return new HeadOfHousehold();
            }
            else if (subType == FAMILY_MEMBER)
            {
                return new FamilyMember();
            }
            return null;
        }

        /// <summary>
        /// A system-generated unique identifier
        /// </summary>
        /// <value>A system-generated unique identifier for a Role</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public bool? Active { get; set; }  // no deletions from DB this is a soft delete.

        public string PersonType { get; set; }  // one of "VOLN" (volunteer), "HOH" (head of household), "FMBR" (family member)

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

        public DateTimeOffset? Dob { get; set; }
    }
}
