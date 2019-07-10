using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.Utils
{
    public interface IPagedResults<T>
    {
        [JsonProperty(PropertyName = "metadata")]
        PaginationMetadata Pagination { get; }

        [JsonProperty(PropertyName = "data")]
        IEnumerable<T> Items { get; }
    }
}
