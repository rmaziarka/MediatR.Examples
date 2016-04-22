namespace KnightFrank.Antares.Domain.Activity.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;
    using KnightFrank.Antares.Domain.Activity.QueryResults;
    using KnightFrank.Antares.Domain.Common.Exceptions;

    using MediatR;
    public class ActivityTypeQueryHandler:IRequestHandler<ActivityTypeQuery, IEnumerable<ActivityTypeQueryResult>>
    {
        private readonly IReadGenericRepository<ActivityTypeDefinition> activityTypeRepository;

        public ActivityTypeQueryHandler(IReadGenericRepository<ActivityTypeDefinition> activityTypeRepository)
        {
            this.activityTypeRepository = activityTypeRepository;
        }

        public IEnumerable<ActivityTypeQueryResult> Handle(ActivityTypeQuery message)
        {
            IEnumerable<ActivityTypeQueryResult> activityTypes = this.activityTypeRepository.Get()
                             .Where(x => x.Country.IsoCode == message.CountryCode && x.PropertyTypeId == message.PropertyTypeId)
                             .OrderBy(x => x.Order)
                             .Select(x => new ActivityTypeQueryResult
                             {
                                 Id = x.ActivityType.Id,
                                 Order = x.Order
                             })
                             .ToList();

            if (!activityTypes.Any())
            {
                throw new DomainValidationException("query", "No configuration found.");
            }

            return activityTypes;
        }
    }
}
