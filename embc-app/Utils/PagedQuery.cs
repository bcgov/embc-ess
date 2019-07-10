using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Utils
{
    public class PagedQuery<T>
    {
        public PaginationMetadata Pagination { get; }
        public IQueryable<T> Query { get; }

        public PagedQuery(IQueryable<T> query, int offset, int limit)
        {
            var itemCount = query.Count();
            double pageSize = limit;
            Pagination = new PaginationMetadata()
            {
                CurrentPage = (int)Math.Floor(offset / pageSize) + 1,
                PageSize = limit,
                TotalCount = itemCount,
                TotalPages = (int)Math.Ceiling(itemCount / pageSize)
            };
            Query = query.Skip(offset).Take(limit);
        }
    }
}
