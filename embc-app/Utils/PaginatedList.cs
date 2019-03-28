using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.Utils
{
    /// <summary>
    /// A list that supports pagination in the form of Limit/Offset
    /// </summary>
    public class PaginatedList<T> : List<T>
    {
        // Have a sensible maximum for the limit parameter...
        private const int maxLimit = 500;

        public int Limit { get; private set; }
        public int Offset { get; private set; }
        public int TotalItemCount { get; private set; }

        public bool HasPreviousPage()
        {
            return GetCurrentPage() > 1;
        }

        public bool HasNextPage()
        {
            return GetCurrentPage() < this.GetTotalPages();
        }

        public int GetCurrentPage()
        {
            double pageSize = (double)this.Limit;
            return (int)Math.Floor(this.Offset / pageSize) + 1;
        }

        public int GetTotalPages()
        {
            double pageSize = (double)this.Limit;
            return (int)Math.Ceiling(this.TotalItemCount / pageSize);
        }

        public PaginatedList(IEnumerable<T> items, int count, int offset, int limit) : base(items)
        {
            Offset = offset;
            Limit = (limit > maxLimit ? maxLimit : limit);
            TotalItemCount = count;
        }
    }
}
