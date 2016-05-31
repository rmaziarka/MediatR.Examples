namespace KnightFrank.Antares.Domain.LatestView.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.LatestView.DataProviders;
    using KnightFrank.Antares.Domain.LatestView.Queries;
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    using MediatR;

    public class LatestViewQueryHandler : IRequestHandler<LatestViewsQuery, IEnumerable<LatestViewQueryResultItem>>
    {
        private readonly IReadGenericRepository<LatestView> latestViewRepository;
        private readonly IReadGenericRepository<User> userRepository;
        private readonly INinjectInstanceResolver ninjectInstanceResolver;

        public LatestViewQueryHandler(
            IReadGenericRepository<LatestView> latestViewRepository,
            IReadGenericRepository<User> userRepository,
            INinjectInstanceResolver ninjectInstanceResolver)
        {
            this.latestViewRepository = latestViewRepository;
            this.userRepository = userRepository;
            this.ninjectInstanceResolver = ninjectInstanceResolver;
        }

        public IEnumerable<LatestViewQueryResultItem> Handle(LatestViewsQuery message)
        {
            // TODO: this needs to be an id of the currently logged-on user
            Guid userId = this.userRepository.Get().First().Id;
            int maxLatestViews = int.Parse(ConfigurationManager.AppSettings["MaxLatestViews"]);

            // get latest views of each entity type, making sure that list contains distinct entities
            List<EntityLatestViews> latestViews =
                this.latestViewRepository.Get()
                    .Where(x => x.UserId == userId)
                    .GroupBy(x => x.EntityTypeString)
                    .Select(x => new EntityLatestViews
                    {
                        EntityTypeCode = x.Key,
                        List =
                            x.GroupBy(lv => lv.EntityId)
                             .Select(gLv => gLv.OrderByDescending(lv => lv.CreatedDate).FirstOrDefault())
                             .Take(maxLatestViews)
                    })
                    .ToList();

            // using dedicated data providers retreive additional data required to properly render latest views entries on UI
            List<LatestViewQueryResultItem> latestViewedEntitiesData =
                latestViews
                    .Select(x =>
                    {
                        var entityType = (EntityTypeEnum)Enum.Parse(typeof(EntityTypeEnum), x.EntityTypeCode, true);
                        ILatestViewDataProvider dataProvider = this.ninjectInstanceResolver.GetLatestViewDataProvider(entityType);
                        return dataProvider.GetData(x);
                    })
                    .ToList();

            return latestViewedEntitiesData;
        }
    }
}
