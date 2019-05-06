using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gov.Jag.Embc.Public.Utils
{
    public interface IPagedResults<T>
    {
        [JsonProperty(PropertyName = "metadata")]
        PaginationMetadata Pagination { get; }

        [JsonProperty(PropertyName = "data")]
        IEnumerable<T> Items { get; }
    }

    /// <summary>
    /// A list that supports pagination in the form of Limit/Offset
    /// </summary>
    public class PaginatedList<T> : IPagedResults<T>
    {
        public PaginationMetadata Pagination { get; }
        public IEnumerable<T> Items { get; }

        public PaginatedList(IEnumerable<T> items, int offset, int limit)
        {
            var itemCount = items.Count();
            double pageSize = limit;
            Pagination = new PaginationMetadata()
            {
                CurrentPage = (int)Math.Floor(offset / pageSize) + 1,
                PageSize = limit,
                TotalCount = itemCount,
                TotalPages = (int)Math.Ceiling(itemCount / pageSize)
            };
            Items = items.Skip(offset)
                .Take(limit)
                .ToArray();
        }
    }
}
