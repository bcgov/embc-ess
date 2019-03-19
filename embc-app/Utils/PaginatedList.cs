using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public PaginatedList(List<T> items, int count, int offset = 0, int limit = 50)
        {
            Offset = offset;
            Limit = (limit > maxLimit ? maxLimit : limit);
            TotalItemCount = count;

            this.AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int offset = 0, int limit = 50)
        {
            var count = await source.CountAsync();
            var items = await source.Skip(offset).Take(limit).ToListAsync();
            return new PaginatedList<T>(items, count, offset, limit);
        }
    }
}
