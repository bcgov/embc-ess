namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    public partial class MicrosoftDynamicsCRMincident
    {

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "customerid_account@odata.bind")]
        public string CustomerIdAccountODataBind { get; set; }

        [JsonProperty(PropertyName = "bcgov_BCSellersAddress@odata.bind")]
        public string BCSellersAddressODataBind { get; set; }
        [JsonProperty(PropertyName = "bcgov_OutsideBCSellersAddress@odata.bind")]
        public string OutsideBCSellersAddressODataBind { get; set; }
        [JsonProperty(PropertyName = "bcgov_ImportersAddress@odata.bind")]
        public string ImportersAddressODataBind { get; set; }
        [JsonProperty(PropertyName = "bcgov_OriginatingSellersAddress@odata.bind")]
        public string OriginatingSellersAddressODataBind { get; set; }
        [JsonProperty(PropertyName = "bcgov_AddressofBusinessthathasGivenorLoaned@odata.bind")]
        public string AddressofBusinessthathasGivenorLoanedODataBind { get; set; }
        [JsonProperty(PropertyName = "bcgov_AddressofBusinessthathasRentedorLeased@odata.bind")]
        public string AddressofBusinessThatHasRentedorLeasedODataBind { get; set; }


        [JsonProperty(PropertyName = "bcgov_EquipmentLocation@odata.bind")]
        public string EquipmentLocationODataBind { get; set; }

        [JsonProperty(PropertyName = "bcgov_Submitter@odata.bind")]
        public string SubmitterODataBind { get; set; }

        [JsonProperty(PropertyName = "bcgov_ApplicationTypeId@odata.bind")]
        public string ApplicationTypeIdODataBind { get; set; }
    }
}
