using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public abstract class Address
    {
        static public Address Create(string subType)
        {
            if (subType == "BCAD")
            {
                return new BcAddress();
            }
            else
            {
                return new OtherAddress();
            }
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
        public string PostalCodeOrZip { get; set; }
        [MaxLength(255)]
        public string CommunityOrCity { get; set; }
        [MaxLength(255)]
        public string ProvinceOrState { get; set; }
        [MaxLength(255)]
        public string Country { get; set; }
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
        public OtherAddress()
        {
            AddressSubtypeCode = "OTAD";
        }
    }
}
