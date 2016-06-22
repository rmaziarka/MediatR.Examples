namespace KnightFrank.Antares.Search.Common.QueryResults
{
    using System.Collections.Generic;

    public class PageableResult<T>
    {
        public long Total { get; set; }

        public int Size { get; set; }

        public int Page { get; set; }

        public ICollection<T> Data { get; set; }
    }
}
