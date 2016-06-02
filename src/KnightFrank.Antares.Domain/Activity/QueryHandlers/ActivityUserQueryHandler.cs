namespace KnightFrank.Antares.Domain.Activity.QueryHandlers
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;

    using MediatR;

    public class ActivityUserQueryHandler : IRequestHandler<ActivityUserQuery, ActivityUser>
    {
        private readonly IReadGenericRepository<ActivityUser> activityUserRepository;

        public ActivityUserQueryHandler(IReadGenericRepository<ActivityUser> activityUserRepository)
        {
            this.activityUserRepository = activityUserRepository;
        }

        public ActivityUser Handle(ActivityUserQuery query)
        {
            ActivityUser result =
                this.activityUserRepository.Get().SingleOrDefault(a => a.Id == query.Id & a.ActivityId == query.ActivityId);

            return result;
        }
    }
}
