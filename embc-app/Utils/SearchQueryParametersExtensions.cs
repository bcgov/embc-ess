using System;
using System.Linq;

namespace Gov.Jag.Embc.Public.Utils
{
    public static class SearchQueryParametersExtensions
    {
        public static bool HasQuery(this SearchQueryParameters qs)
        {
            return !String.IsNullOrEmpty(qs.Query);
        }

        public static bool HasSortBy(this SearchQueryParameters qs)
        {
            return !String.IsNullOrEmpty(qs.SortBy);
        }
    }
}
