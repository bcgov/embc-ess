namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    public partial class MicrosoftDynamicsCRMcontact
    {
        
        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "parentcustomerid_account@odata.bind")]
        public string ParentCustomerIdAccountODataBind { get; set; }

    }
}
