namespace KnightFrank.Antares.Domain.LatestView.DataProviders
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Model.LatestView;

    public class EntityLatestViews
    {
        public EntityTypeEnum EntityType { get; set; }
        public IEnumerable<LatestView> List { get; set; }
    }
}