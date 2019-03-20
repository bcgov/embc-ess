using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;

namespace Gov.Jag.Embc.Public.Utils
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string sortBy)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(sortBy))
            {
                throw new ArgumentNullException("sortBy");
            }

            // support sorting by multiple fields; e.g. "+name,-age" ==> Sort by Name ASC, then Age DESC
            var listSortBy = sortBy.Split(",");
            listSortBy = listSortBy.Select(item => AdjustDirection(item)).ToArray();

            var sortExpression = string.Join(",", listSortBy);

            try
            {
                source = source.OrderBy(sortExpression);
            }
            catch (ParseException ex)
            {
                // sortBy included field not part of the model - Ignore
            }

            return source;
        }

        private static string AdjustDirection(string item)
        {
            // no direction specified (assume ascending order)
            var direction = "+";
            var field = item;

            // parse direction and field from provided sort-item
            // e.g. "name" ==> direction: ASC, field: name
            // e.g. "-name" ==> direction: DESC, field: name
            if (item.StartsWith("-"))
            {
                direction = "-";
                field = item.Substring(1);
            }

            switch (direction)
            {
                case "+":
                    return field + " ascending";

                case "-":
                    return field + " descending";

                default:
                    return field;
            };
        }
    }
}
