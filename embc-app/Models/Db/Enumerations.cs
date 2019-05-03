using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class Enumerations
    {
        public enum FamilyRelationshipTypes
        {
            [DisplayName("HOH")]
            [Description("Head of Household")]
            HeadOfHousehold,
            [DisplayName("IMMF")]
            [Description("Immediate Family")]
            ImmediateFamily,
            [DisplayName("EXTF")]
            [Description("Extended Family")]
            ExtendedFamily
        }

        public enum AddressType
        {
            [DisplayName("Primary")]
            Primary,
            [DisplayName("Mailing")]
            Mailing
        }

        public enum AddressSubType
        {
            [DisplayName("BCAD")]
            BCAddress,
            [DisplayName("OTAD")]
            OtherAddress
        }
    }
}
