using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class Supplier : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool Active { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(255)]
        public string City { get; set; }

        [MaxLength(50)]
        public string Province { get; set; }

        [MaxLength(50)]
        public string PostalCode { get; set; }

        [MaxLength(50)]
        public string Telephone { get; set; }

        [MaxLength(50)]
        public string Fax { get; set; }
    }
}
