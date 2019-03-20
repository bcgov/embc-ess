using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gov.Jag.Embc.Public.Utils
{
    public class PaginationMetadata
    {
        [JsonProperty("page")]
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        // TODO: we might want to do pagination links in future releases?
        // see Best Practice #9 --> https://saipraveenblog.wordpress.com/2014/09/29/rest-api-best-practices/
        public List<PaginationLink> Links { get; set; }
    }
}
