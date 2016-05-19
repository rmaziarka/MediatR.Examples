namespace KnightFrank.Antares.Domain.AreaBreakdown.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AreaBreakdown.Queries;

    using MediatR;
    public class AreaBreakdownQueryHandler : IRequestHandler<AreaBreakdownQuery, IList<PropertyAreaBreakdown>>
    {
        private readonly IReadGenericRepository<PropertyAreaBreakdown> areaBreakdownRepository;

        public AreaBreakdownQueryHandler(IReadGenericRepository<PropertyAreaBreakdown> areaBreakdownRepository)
        {
            this.areaBreakdownRepository = areaBreakdownRepository;
        }

        public IList<PropertyAreaBreakdown> Handle(AreaBreakdownQuery query)
        {
            IQueryable<PropertyAreaBreakdown> areaBreakdownQuery = this.areaBreakdownRepository.Get().Where(x => x.PropertyId == query.PropertyId);

            if (query.AreaIds != null && query.AreaIds.Any())
            {
                areaBreakdownQuery = areaBreakdownQuery.Where(x => query.AreaIds.Contains(x.Id));
            }

            return areaBreakdownQuery.ToList();
        }
    }
}