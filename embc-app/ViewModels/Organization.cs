using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class Organization
    {
        public string Id { get; set; }
        public bool? Active { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public string BCeIDBusinessGuid { get; set; }

        public Region Region { get; set; }

        public RegionalDistrict RegionalDistrict { get; set; }

        public Community Community { get; set; }

        public string AdminFirstName { get; set; }

        public string AdminLastName { get; set; }

        public string AdminBCeID { get; set; }

        public string LegalName { get; set; }
    }
}
