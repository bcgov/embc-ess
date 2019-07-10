using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Utils
{
    public class PagedResult<T> : IPagedResults<T>
    {
        public PaginationMetadata Pagination { get; }
        public IEnumerable<T> Items { get; }
        public IList<T> Results { get; set; }

        public PagedResult(IEnumerable<T> items, PaginationMetadata pagination)
        {
            Pagination = pagination;
            Results = items.ToList();
        }
    }
}
