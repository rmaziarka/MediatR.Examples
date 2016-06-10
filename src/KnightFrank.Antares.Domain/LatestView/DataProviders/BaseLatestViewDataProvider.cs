namespace KnightFrank.Antares.Domain.LatestView.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    public abstract class BaseLatestViewDataProvider<T> : ILatestViewDataProvider where T : BaseEntity
    {
        /// <summary>
        /// Gets the queryable for the currently handled entity type.
        /// </summary>
        /// <returns></returns>
        public abstract IQueryable<T> GetEntitiesWithIncludes();

        /// <summary>
        /// Gets the latest view data for entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="latestView">The latest view.</param>
        /// <returns></returns>
        public abstract LatestViewData CreateLatestViewData(T entity, LatestView latestView);

        /// <summary>
        /// Gets the latest view data the currently handled entity type.
        /// </summary>
        /// <param name="entityLatestViews">The x.</param>
        /// <returns></returns>
        public LatestViewQueryResultItem GetData(EntityLatestViews entityLatestViews)
        {
            Guid[] entityIds = entityLatestViews.List.Select(e => e.EntityId).ToArray();
            List<T> entityList =
                this.GetEntitiesWithIncludes()
                .Where(qe => entityIds.Contains(qe.Id))
                .ToList();

            /*
             *  EntityList contains entities that are on the list of recent views.
             *  Now we need to prepare additional data for each entity type and afterwards sort them by recent view date.
             *  And that's why we need to join both collections as one of them contains data that we are interested in
             *  and the other contains sort order.
             */
            return new LatestViewQueryResultItem
            {
                EntityTypeCode = entityLatestViews.EntityTypeCode,
                List = entityList
                    .Join(entityLatestViews.List, p => p.Id, lv => lv.EntityId, this.CreateLatestViewData)
                    .OrderByDescending(e => e.CreatedDate)
            };
        }
    }
}