
namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    public partial class MicrosoftDynamicsCRMaccount
    {
        
        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "adoxio_account_adoxio_legalentity_Account@odata.bind")]
        public string AdoxioAccountAdoxioLegalentityAccountODataBind { get; set; }
       
        [JsonProperty(PropertyName = "primarycontactid@odata.bind")]
        public string PrimaryContactidODataBind { get; set; }

        [JsonProperty(PropertyName = "bcgov_AdditionalContact@odata.bind")]
        public string AdditionalContactODataBind { get; set; }

        [JsonProperty(PropertyName = "bcgov_CurrentBusinessMailingAddress@odata.bind")]
        public string CurrentBusinessMailingAddressODataBind { get; set; }

        [JsonProperty(PropertyName = "bcgov_CurrentBusinessPhysicalAddress@odata.bind")]
        public string CurrentBusinessPhysicalAddressODataBind { get; set; }

    }
}
