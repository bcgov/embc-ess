
namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;


    public partial class MicrosoftDynamicsCRMbcgovBusinesscontact
    {
        
        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "bcgov_Contact@odata.bind")]
        public string ContactODataBind { get; set; }

        [JsonProperty(PropertyName = "bcgov_BusinessProfile@odata.bind")]
        public string AccountODataBind { get; set; }

    }
}
