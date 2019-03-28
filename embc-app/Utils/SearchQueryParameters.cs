using Microsoft.AspNetCore.Mvc;

namespace Gov.Jag.Embc.Public.Utils
{
    /// <summary>
    /// A class that models the query parameters to enable Pagination/Searching/Sorting
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

        // NOTE: The following properties differ from what we get in the query string but that's okay.
        // We can use the [FromQuery] attribute to tell ASP NET Core how to map them to our model
        // http://www.devcode4.com/article/asp-net-core-bind-query-parameter-with-different-name

        // SEARCH params
        [FromQuery(Name = "q")]
        public string Query { get; set; }

        // SORTING params
        [FromQuery(Name = "sort")]
        public string SortBy { get; set; }  // e.g. "+name" for ASC, "-name" for DESC

        [FromQuery(Name = "notactive")]
        public bool IncludeDeactivated { get; set; } = false;
    }
}
