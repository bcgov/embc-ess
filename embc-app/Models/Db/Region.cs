using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class Region : IAuditableEntity
    {
        [Key]
        [MaxLength(255)]
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
