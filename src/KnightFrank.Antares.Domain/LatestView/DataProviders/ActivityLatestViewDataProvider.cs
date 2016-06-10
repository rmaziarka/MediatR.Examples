namespace KnightFrank.Antares.Domain.LatestView.DataProviders
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    public class ActivityLatestViewDataProvider : BaseLatestViewDataProvider<Activity>
    {
        private readonly IReadGenericRepository<Activity> activityRepository;

        public ActivityLatestViewDataProvider(IReadGenericRepository<Activity> activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public override IQueryable<Activity> GetEntitiesWithIncludes()
        {
            return this.activityRepository.GetWithInclude(x => x.Property.Address);
        }

        public override LatestViewData CreateLatestViewData(Activity activity, LatestView latestView)
        {
            return new LatestViewData()
            {
                Id = latestView.EntityId,
                CreatedDate = latestView.CreatedDate,
                Data = activity.Property.Address
            };
        }
    }
}
