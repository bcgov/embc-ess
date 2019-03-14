using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public abstract class Address
    {
        // Factory method
        static public Address Create(string subType)
        {
            if (subType == "BCAD")
            {
                return new BcAddress();
            }
            else if (subType == "OTAD")
            {
                return new OtherAddress();
            }
            return null;
        }

        public string Id { get; set; }
        public string AddressSubtypeCode { get; set; }  // one of ['BCAD', 'OTAD'] for BC vs non-BC addresses
        [MaxLength(255)]
        public string AddressLine1 { get; set; }
        [MaxLength(255)]
        public string AddressLine2 { get; set; }
        [MaxLength(255)]
        public string AddressLine3 { get; set; }
        [MaxLength(255)]
        public string PostalCode { get; set; }
    }

    public class BcAddress : Address
    {
        // related entities
        public Community Community { get; set; }

        public BcAddress()
        {
            AddressSubtypeCode = "BCAD";
        }
    }

    public class OtherAddress : Address
    {

        [MaxLength(255)]
        public string City { get; set; }

        [MaxLength(255)]
        public string Province { get; set; }

        [MaxLength(255)]
        public Country Country { get; set; }

        public OtherAddress()
        {
            AddressSubtypeCode = "OTAD";
        }
    }
}
