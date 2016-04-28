namespace KnightFrank.Antares.Domain.Activity.QueryHandlers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;
    using KnightFrank.Antares.Domain.Activity.QueryResults;

    using MediatR;

    public class ActivitiesQueryHandler : IRequestHandler<ActivitiesQuery, IEnumerable<ActivitiesQueryResult>>
    {
        private readonly IReadGenericRepository<Activity> activityRepository;

        public ActivitiesQueryHandler(IReadGenericRepository<Activity> activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public IEnumerable<ActivitiesQueryResult> Handle(ActivitiesQuery message)
        {
            return this.activityRepository.Get().Include(a => a.Property.Address).Select(x => new ActivitiesQueryResult
            {
                Id = x.Id,
                PropertyName = x.Property.Address.PropertyName,
                PropertyNumber = x.Property.Address.PropertyNumber,
                Line2 = x.Property.Address.Line2
            }).ToList();
        }
    }
}
