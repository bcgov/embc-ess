
namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    public partial class MicrosoftDynamicsCRMbcgovLocation
    {
        
        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "bcgov_LocationAddress@odata.bind")]
        public string LocationAddressODataBind { get; set; }

        [JsonProperty(PropertyName = "bcgov_BusinessProfile@odata.bind")]
        public string BusinessProfileODataBind { get; set; }      
    }
}
