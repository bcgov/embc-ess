namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    public partial class MicrosoftDynamicsCRMadoxioTiedhouseconnection
    {
        
        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "adoxio_AccountId@odata.bind")]
        public string AccountODataBind { get; set; }

    }
}
