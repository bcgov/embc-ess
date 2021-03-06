// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// RetrieveMetadataChangesResponse
    /// </summary>
    public partial class MicrosoftDynamicsCRMRetrieveMetadataChangesResponse
    {
        /// <summary>
        /// Initializes a new instance of the
        /// MicrosoftDynamicsCRMRetrieveMetadataChangesResponse class.
        /// </summary>
        public MicrosoftDynamicsCRMRetrieveMetadataChangesResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// MicrosoftDynamicsCRMRetrieveMetadataChangesResponse class.
        /// </summary>
        public MicrosoftDynamicsCRMRetrieveMetadataChangesResponse(string serverVersionStamp = default(string), MicrosoftDynamicsCRMDeletedMetadataCollection deletedMetadata = default(MicrosoftDynamicsCRMDeletedMetadataCollection), IList<MicrosoftDynamicsCRMComplexEntityMetadata> entityMetadata = default(IList<MicrosoftDynamicsCRMComplexEntityMetadata>))
        {
            ServerVersionStamp = serverVersionStamp;
            DeletedMetadata = deletedMetadata;
            EntityMetadata = entityMetadata;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ServerVersionStamp")]
        public string ServerVersionStamp { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "DeletedMetadata")]
        public MicrosoftDynamicsCRMDeletedMetadataCollection DeletedMetadata { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "EntityMetadata")]
        [NotMapped] public IList<MicrosoftDynamicsCRMComplexEntityMetadata> EntityMetadata { get; set; }

    }
}
