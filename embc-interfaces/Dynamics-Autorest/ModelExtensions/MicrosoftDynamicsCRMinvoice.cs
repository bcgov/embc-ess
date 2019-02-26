namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    public partial class MicrosoftDynamicsCRMinvoice
    {
        
        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "customerid_account@odata.bind")]
        public string CustomerIdAccountODataBind { get; set; }

    }
}
