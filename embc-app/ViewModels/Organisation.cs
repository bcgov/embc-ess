using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gov.Jag.Embc.Public.ViewModels
{

    public enum Adoxio_stateprovince
    {
        [EnumMember(Value = "Alberta")]
        AB = 845280000,
        [EnumMember(Value = "British Columbia")]
        BC,
        [EnumMember(Value = "Manitoba")]
        MN,
        [EnumMember(Value = "New Brunswick")]
        NB,
        [EnumMember(Value = "Newfoundland and Labrador")]
        NL,
        [EnumMember(Value = "Northwest Territories")]
        NT,
        [EnumMember(Value = "Nova Scotia")]
        NS,
        [EnumMember(Value = "Nunavut")]
        NU,
        [EnumMember(Value = "Ontario")]
        ON,
        [EnumMember(Value = "Prince Edward Island")]
        PE,
        [EnumMember(Value = "Quebec")]
        QC,
        [EnumMember(Value = "Saskatchewan")]
        SK,
        [EnumMember(Value = "Yukon")]
        YT
    }

    public enum BusinessTypeEnum
    {
        [EnumMember(Value = "Sole Proprietorship")]
        SoleProprietorship = 1,
        [EnumMember(Value = "Society")]
        Society = 931490000,
        [EnumMember(Value = "Partnership")]
        Partnership = 931490001,
        [EnumMember(Value = "Private Corporation")]
        PrivateCorporation = 931490002,
        [EnumMember(Value = "Public Corporation")]
        PublicCorporation = 931490003
    }

    public class Organisation
    {
        public string id { get; set; } // Guid

        //public string clientId { get; set; } // dynamics = accountnumber
        public string bcIncorporationNumber { get; set; }
        public string businessDBAName { get; set; } // dynamics = bcgov_doingbusinessasdbaname
        public string businessEmail { get; set; } //dynamics = emailaddress1
        public bool? consentForEmailCommunication { get; set; }

        public string businessLegalName { get; set; } // dynamics = name        
        public string businessNumber { get; set; } // dynamics = bcgov_businessnumber
        public string businessType { get; set; } // dynamics = businesstypecode

        public DateTimeOffset? dateOfIncorporationInBC { get; set; }

        public string externalId { get; set; }
                
        
        public string businessPhoneNumber { get; set; } //dynamics = telephone1
        public string description { get; set; } //dynamics = description

        public string physicalAddressName { get; set; } //dynamics = address1_name
        public string physicalAddressLine1 { get; set; } //dynamics = address1_line1
        public string physicalAddressLine2 { get; set; } //dynamics = address1_line1
        public string physicalAddressCity { get; set; } //dynamics = address1_city
        public string physicalAddressCountry { get; set; } //dynamics = address1_country
        public string physicalAddressProvince { get; set; } //dynamics = address1_stateorprovince
        public string physicalAddressPostalCode { get; set; } //dynamics = address1_postalcode

        public string mailingAddressName { get; set; } //dynamics = address2_name
        public string mailingAddressLine1 { get; set; } //dynamics = address2_line1
        public string mailingAddressLine2 { get; set; } //dynamics = address2_line1
        public string mailingAddressCity { get; set; } //dynamics = address2_city
        public string mailingAddressCountry { get; set; } //dynamics = address2_country
        public string mailingAddressProvince { get; set; } //dynamics = address2_stateorprovince
        public string mailingAddressPostalCode { get; set; } //dynamics = address2_postalcode
        public bool? foippaconsent { get; set; } //dynamics = address2_postalcode
        public bool? declarationofcorrectinformation { get; set; } //dynamics = address2_postalcode
        
        public ViewModels.Person additionalContact { get; set; }
        public ViewModels.Person primaryContact { get; set; }

        //public ViewModels.CustomAddress physicalAddress { get; set; }
        //public ViewModels.CustomAddress mailingAddress { get; set; }

        public string name { get; set; }
        public string pstNumber { get; set; }
        public string websiteAddress { get; set; }

        [JsonProperty(PropertyName = "submittedDate")]
        public System.DateTimeOffset? SubmittedDate { get; set; }
    }
}