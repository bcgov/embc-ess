using System;
using System.Linq;

namespace Gov.Jag.Embc.Public.Utils
{
    public static class PaginatedListExtensions
    {
        public static bool HasPreviousPage<T>(this PaginatedList<T> list)
        {
            return list.GetCurrentPage() > 1;
        }

        public static bool HasNextPage<T>(this PaginatedList<T> list, int totalCount)
        {
            return list.GetCurrentPage() < list.GetTotalPages();
        }

        public static int GetCurrentPage<T>(this PaginatedList<T> list)
        {
            double pageSize = (double)list.Limit;
            return (int)Math.Floor(list.Offset / pageSize) + 1;
        }

        public static int GetTotalPages<T>(this PaginatedList<T> list)
        {
            double pageSize = (double)list.Limit;
            return (int)Math.Ceiling(list.TotalItemCount / pageSize);
        }
    }
}
