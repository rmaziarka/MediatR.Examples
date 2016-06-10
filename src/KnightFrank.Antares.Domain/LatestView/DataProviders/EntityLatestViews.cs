namespace KnightFrank.Antares.Domain.LatestView.DataProviders
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.LatestView;

    public class EntityLatestViews
    {
        public string EntityTypeCode { get; set; }
        public IEnumerable<LatestView> List { get; set; }
    }
}