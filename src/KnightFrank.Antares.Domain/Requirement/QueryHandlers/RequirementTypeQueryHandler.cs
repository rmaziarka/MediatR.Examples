namespace KnightFrank.Antares.Domain.Activity.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;
    using KnightFrank.Antares.Domain.Activity.QueryResults;
    using KnightFrank.Antares.Domain.Common.Exceptions;

    using MediatR;
    public class RequirementTypeQueryHandler : IRequestHandler<RequirementTypeQuery, IEnumerable<RequirementTypeQueryResult>>
    {
        private readonly IReadGenericRepository<RequirementType> requirementTypeRepository;

        public RequirementTypeQueryHandler(IReadGenericRepository<RequirementType> requirementTypeRepository)
        {
            this.requirementTypeRepository = requirementTypeRepository;
        }

        public IEnumerable<RequirementTypeQueryResult> Handle(RequirementTypeQuery message)
        {
            IEnumerable<RequirementTypeQueryResult> requirementTypes = this.requirementTypeRepository.Get()
                             .Select(x => new RequirementTypeQueryResult
                             {
                                 Id = x.Id
                             })
                             .ToList();

            return requirementTypes;
        }
    }
}
