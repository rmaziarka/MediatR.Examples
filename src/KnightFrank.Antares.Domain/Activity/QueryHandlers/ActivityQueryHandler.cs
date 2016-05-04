namespace KnightFrank.Antares.Domain.Activity.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;

    using MediatR;

    public class ActivityQueryHandler : IRequestHandler<ActivityQuery, Activity>
    {
        private readonly IReadGenericRepository<Activity> activityRepository;

        public ActivityQueryHandler(IReadGenericRepository<Activity> activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public Activity Handle(ActivityQuery query)
        {
            Activity result =
                this.activityRepository.Get()
                    .Include(a => a.Property)
                    .Include(a => a.Property.Address)
                    .Include(a => a.Contacts)
                    .Include(a => a.Attachments)
                    .SingleOrDefault(a => a.Id == query.Id);

            return result;
        }
    }
}
