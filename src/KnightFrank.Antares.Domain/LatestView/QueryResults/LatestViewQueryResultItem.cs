namespace KnightFrank.Antares.Domain.LatestView.QueryResults
{
    using System.Collections.Generic;

    public class LatestViewQueryResultItem
    {
        public string EntityTypeCode { get; set; }
        public IEnumerable<LatestViewData> List { get; set; }
    }
}