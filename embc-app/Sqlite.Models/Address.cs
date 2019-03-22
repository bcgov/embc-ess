using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Sqlite.Models
{
    /// <summary>
    /// Address Database Model
    /// </summary>
    public abstract partial class Address
    {
        public const string BC_ADDRESS = "BCAD";
        public const string OTHER_ADDRESS = "OTAD";

        // Factory method
        public static Address Create(string subType)
        {
            if (subType == BC_ADDRESS)
            {
                return new BcAddress();
            }
            else if (subType == OTHER_ADDRESS)
            {
                return new OtherAddress();
            }
            return null;
        }

        /// <summary>
        /// A system-generated unique identifier for a Role
        /// </summary>
        /// <value>A system-generated unique identifier for a Role</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public string AddressSubtype { get; set; }  // one of ['BCAD', 'OTAD'] for BC vs non-BC addresses

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string PostalCode { get; set; }
    }

    public sealed partial class BcAddress : Address
    {
        public Guid CommunityId { get; set; }

        public Community Community { get; set; }

        public BcAddress()
        {
            AddressSubtype = Address.BC_ADDRESS;
        }
    }

    public sealed partial class OtherAddress : Address
    {
        public string City { get; set; }

        public string Province { get; set; }

        public Guid CountryId { get; set; }

        public Country Country { get; set; }

        public OtherAddress()
        {
            AddressSubtype = Address.OTHER_ADDRESS;
        }
    }
}
