namespace KnightFrank.Antares.Domain.LatestView.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.LatestView.DataProviders;
    using KnightFrank.Antares.Domain.LatestView.Queries;
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    using MediatR;

    public class LatestViewQueryHandler : IRequestHandler<LatestViewsQuery, IEnumerable<LatestViewQueryResultItem>>
    {
        private readonly IReadGenericRepository<LatestView> latestViewRepository;
        private readonly IReadGenericRepository<User> userRepository;
        private readonly Func<EntityTypeEnum, ILatestViewDataProvider> latestViewDataProvider;

        public LatestViewQueryHandler(
            IReadGenericRepository<LatestView> latestViewRepository, 
            IReadGenericRepository<User> userRepository,  
            Func<EntityTypeEnum, ILatestViewDataProvider> latestViewDataProvider)
        {
            this.latestViewRepository = latestViewRepository;
            this.userRepository = userRepository;
            this.latestViewDataProvider = latestViewDataProvider;
        }

        public IEnumerable<LatestViewQueryResultItem> Handle(LatestViewsQuery message)
        {
            // TODO: this needs to be an id of the currently logged-on user
            Guid userId = this.userRepository.Get().First().Id;

            List<EntityLatestViews> latestViews =
                this.latestViewRepository.Get()
                    .Where(x => x.UserId == userId)
                    .GroupBy(x => x.EntityType)
                    .Select(x => new EntityLatestViews
                    {
                        EntityType = x.Key,
                        List =
                            x.GroupBy(lv => lv.EntityId)
                             .Select(gLv => gLv.OrderByDescending(lv => lv.CreatedDate).FirstOrDefault())
                             .Take(10)
                             .ToList()
                    })
                    .ToList();

            List<LatestViewQueryResultItem> latestViewedEntitiesData =
                latestViews
                    .Select(x => this.latestViewDataProvider(x.EntityType)?.GetData(x))
                    .Where(x => x != null)
                    .ToList();

            return latestViewedEntitiesData;
        }
    }
}
