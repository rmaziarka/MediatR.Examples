namespace KnightFrank.Antares.Domain.Activity.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Portal;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;

    using MediatR;

    public class ActivityPortalsQueryHandler : IRequestHandler<ActivityPortalsQuery, IEnumerable<Portal>>
    {
        private readonly IReadGenericRepository<PortalDefinition> portalDefinitionRepository;

        public ActivityPortalsQueryHandler(IReadGenericRepository<PortalDefinition> portalDefinitionRepository)
        {
            this.portalDefinitionRepository = portalDefinitionRepository;
        }

        public IEnumerable<Portal> Handle(ActivityPortalsQuery query)
        {
            IQueryable<Portal> result =
                this.portalDefinitionRepository
                    .GetWithInclude(x => x.Portal)
                    .Where(x => x.Country.IsoCode == query.CountryCode)
                    .Select(x => x.Portal);

            return result;
        }
    }
}
