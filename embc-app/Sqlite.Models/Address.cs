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
        public static Address Create(string subType)
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

        /// <summary>
        /// A system-generated unique identifier for a Role
        /// </summary>
        /// <value>A system-generated unique identifier for a Role</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public string AddressSubtypeCode { get; set; }  // one of ['BCAD', 'OTAD'] for BC vs non-BC addresses

        [MaxLength(255)]
        public string AddressLine1 { get; set; }

        [MaxLength(255)]
        public string AddressLine2 { get; set; }

        [MaxLength(255)]
        public string AddressLine3 { get; set; }

        [MaxLength(255)]
        public string CommunityOrCity { get; set; }

        [MaxLength(255)]
        public string Province { get; set; }

        [MaxLength(255)]
        public string PostalCode { get; set; }

        [MaxLength(255)]
        public string Country { get; set; }
    }

    public sealed partial class BcAddress : Address
    {
        public BcAddress()
        {
            AddressSubtypeCode = "BCAD";
        }
    }

    public sealed partial class OtherAddress : Address
    {
        public OtherAddress()
        {
            AddressSubtypeCode = "OTAD";
        }
    }
}
