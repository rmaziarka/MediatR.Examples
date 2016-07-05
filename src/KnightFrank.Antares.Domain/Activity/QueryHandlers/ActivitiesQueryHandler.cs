namespace KnightFrank.Antares.Domain.Activity.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;
    using KnightFrank.Antares.Domain.Activity.QueryResults;

    using MediatR;

    public class ActivitiesQueryHandler : IRequestHandler<ActivitiesFilterQuery, IEnumerable<ActivitiesQueryResult>>
    {
        private readonly IReadGenericRepository<Activity> activityRepository;
        private readonly IReadGenericRepository<RequirementType> requirementTypeRepository;

        public ActivitiesQueryHandler(IReadGenericRepository<Activity> activityRepository, IReadGenericRepository<RequirementType> requirementTypeRepository)
        {
            this.activityRepository = activityRepository;
            this.requirementTypeRepository = requirementTypeRepository;
        }

        public IEnumerable<ActivitiesQueryResult> Handle(ActivitiesFilterQuery message)
        {
            List<Guid> allowedActivityTypes = new List<Guid>();
            if (message.RequirementTypeId != Guid.Empty)
            {
                allowedActivityTypes =
                    this.requirementTypeRepository.Get()
                        .Where(rt => rt.Id == message.RequirementTypeId)
                        .SelectMany(rt => rt.ActivityTypeDefinitions)
                        .Select(atd => atd.ActivityTypeId).Distinct().ToList();
            }
            else
            {
                return this.activityRepository.Get().Include(a => a.Property.Address).Select(x => new ActivitiesQueryResult
                {
                    Id = x.Id,
                    PropertyName = x.Property.Address.PropertyName,
                    PropertyNumber = x.Property.Address.PropertyNumber,
                    Line2 = x.Property.Address.Line2
                }).ToList();
            }

            return this.activityRepository.Get().Include(a => a.Property.Address).Where(a => allowedActivityTypes.Contains(a.ActivityTypeId)).Select(x => new ActivitiesQueryResult
            {
                Id = x.Id,
                PropertyName = x.Property.Address.PropertyName,
                PropertyNumber = x.Property.Address.PropertyNumber,
                Line2 = x.Property.Address.Line2
            }).ToList();
        }
    }
}
