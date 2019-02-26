namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    public partial class MicrosoftDynamicsCRMsharepointdocumentlocation
    {

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "regardingobjectid_adoxio_application@odata.bind")]
        public string RegardingobjectidApplicationODataBind { get; set; }

        [JsonProperty(PropertyName = "parentsiteorlocation_sharepointdocumentlocation@odata.bind")]        
        public string ParentsiteorlocationSharepointdocumentlocationODataBind { get; set; }

    }
}
