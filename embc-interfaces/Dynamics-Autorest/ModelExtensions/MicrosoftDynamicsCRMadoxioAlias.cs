namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    public partial class MicrosoftDynamicsCRMadoxioAlias
    {

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "adoxio_ContactId@odata.bind")]
        public string ContactIdODataBind { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "adoxio_WorkerId@odata.bind")]
        public string WorkerIdODataBind { get; set; }

    }
}
