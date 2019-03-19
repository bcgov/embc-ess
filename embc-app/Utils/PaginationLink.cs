using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gov.Jag.Embc.Public.Utils
{
    public class PaginationLink
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }

        public PaginationLink(string href, string rel, string method = "GET")
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
