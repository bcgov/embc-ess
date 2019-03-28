using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class FamilyRelationshipType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string Code { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public bool Active { get; set; }
    }
}
