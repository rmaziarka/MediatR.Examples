namespace KnightFrank.Antares.Domain.LatestView.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    public abstract class BaseLatestViewDataProvider : ILatestViewDataProvider
    {
        /// <summary>
        /// Gets the queryable for the currently handled entity type.
        /// </summary>
        /// <returns></returns>
        public abstract IQueryable GetEntitiesWithIncludes();

        /// <summary>
        /// Gets the latest view data for entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="latestView">The latest view.</param>
        /// <returns></returns>
        public abstract LatestViewData CreateLatestViewData(BaseEntity entity, LatestView latestView);

        /// <summary>
        /// Gets the latest view data the currently handled entity type.
        /// </summary>
        /// <param name="entityLatestViews">The x.</param>
        /// <returns></returns>
        public LatestViewQueryResultItem GetData(EntityLatestViews entityLatestViews)
        {
            Guid[] entityIds = entityLatestViews.List.Select(e => e.EntityId).ToArray();
            List<BaseEntity> entityList =
                ((IQueryable<BaseEntity>)this.GetEntitiesWithIncludes())
                .Where(qe => entityIds.Contains(qe.Id))
                .ToList();

            return new LatestViewQueryResultItem
            {
                EntityTypeCode = entityLatestViews.EntityType.ToString(),
                List = entityList
                    .Join(entityLatestViews.List, p => p.Id, lv => lv.EntityId, this.CreateLatestViewData)
                    .OrderByDescending(e => e.CreatedDate)
            };
        }
    }
}