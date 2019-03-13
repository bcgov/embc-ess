using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.Sqlite.Models
{
    /// <summary>
    /// Person Database Model
    /// </summary>
    public sealed partial class FamilyMember : Person
    {
        [MaxLength(255)]
        public string RelationshipToEvacuee { get; set; }
        public bool SameLastNameAsEvacuee { get; set; }

        public FamilyMember()
        {
            PersonType = "FMBR";
        }
    }
}
