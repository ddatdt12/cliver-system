using System.Collections;

namespace CliverSystem.DTOs.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public PagedList(List<T> items, int totalCount, int offset, int limit)
        {
            MetaData = new MetaData
            {
                TotalCount = totalCount,
                Count = items.Count,
                Offset = offset,
                Limit = limit
            };
            AddRange(items);
        }
        public static PagedList<T> ToPagedList(IEnumerable<T> source, int totalCount, int offset, int limit)
        {
            var items = source
            .Skip(offset)
            .Take(limit).ToList();

            return new PagedList<T>(items, totalCount, offset, limit);
        }

    }
}
