using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class Address
    {
        // base props
        public string Id { get; set; }
        public string AddressSubtype { get; set; }  // one of ['BCAD', 'OTAD'] for BC vs non-BC addresses
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string PostalCode { get; set; }

        // BC address props
        public Community Community { get; set; }

        // other address props
        public string City { get; set; }
        public string Province { get; set; }
        public Country Country { get; set; }

        public bool isBcAddress => this.AddressSubtype == "BCAD";
        public bool isOtherAddress => this.AddressSubtype == "OTAD";
    }
}
