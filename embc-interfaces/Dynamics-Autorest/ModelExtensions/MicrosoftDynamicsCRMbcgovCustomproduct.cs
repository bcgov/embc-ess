
namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    public partial class MicrosoftDynamicsCRMbcgovCustomproduct
    {
        
        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "bcgov_RelatedApplication@odata.bind")]
        public string RelatedApplicationODataBind { get; set; }
       

    }
}
