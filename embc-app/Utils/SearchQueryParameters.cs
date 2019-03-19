using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gov.Jag.Embc.Public.Utils
{
    /// <summary>
    /// A
    /// </summary>
    public class SearchQueryParameters
    {
        // Have a sensible maximum for the limit parameter...
        private const int MaxLimit = 500;
        private int limit = 50;

        // PAGINATION params
        public int Offset { get; set; } = 0;
        public int Limit
        {
            get => this.limit;
            set => this.limit = value > MaxLimit ? MaxLimit : value;
        }

        // SEARCH params
        [JsonProperty("q")]
        public string Query { get; set; }

        // SORTING params
        [JsonProperty("sort")]
        public string SortBy { get; set; }  // e.g. "+name" for ASC, "-name" for DESC
    }
}
