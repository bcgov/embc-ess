using System;
using System.Linq;

namespace Gov.Jag.Embc.Public.Utils
{
    public static class SearchQueryParametersExtensions
    {
        public static bool HasPrevious(this SearchQueryParameters SearchQueryParameters)
        {
            return (SearchQueryParameters.Page > 1);
        }

        public static bool HasNext(this SearchQueryParameters SearchQueryParameters, int totalCount)
        {
            return (SearchQueryParameters.Page < (int)GetTotalPages(SearchQueryParameters, totalCount));
        }

        public static double GetTotalPages(this SearchQueryParameters SearchQueryParameters, int totalCount)
        {
            return Math.Ceiling(totalCount / (double)SearchQueryParameters.PageCount);
        }

        public static bool HasQuery(this SearchQueryParameters SearchQueryParameters)
        {
            return !String.IsNullOrEmpty(SearchQueryParameters.Query);
        }

        public static bool IsDescending(this SearchQueryParameters SearchQueryParameters)
        {
            if (!String.IsNullOrEmpty(SearchQueryParameters.OrderBy))
            {
                return SearchQueryParameters.OrderBy.Split(' ').Last().ToLowerInvariant().StartsWith("desc");
            }
            return false;
        }
    }
}
