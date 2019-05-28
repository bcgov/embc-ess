using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class EvacueeRegistrationAudit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EventId { get; set; }

        [Required]
        public long EssFileNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Action { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [MaxLength(100)]
        public string User { get; set; }

        [Required]
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
    }
}
