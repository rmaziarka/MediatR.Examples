namespace KnightFrank.Antares.Domain.Property.QueryHandlers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Property.Queries;

    using MediatR;

    public class PropertiesQueryHandler : IRequestHandler<PropertiesQuery, IEnumerable<Property>>
    {
        private readonly IReadGenericRepository<Property> propertyRepository;

        public PropertiesQueryHandler(IReadGenericRepository<Property> propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public IEnumerable<Property> Handle(PropertiesQuery message)
        {
            var result =
                this.propertyRepository.Get()
                    .Include(p => p.Ownerships.Select(o => o.Contacts))
                    .Include(p => p.Ownerships.Select(o => o.OwnershipType))
                    .Include(p => p.Address);
            
            return result;
        }
    }
}
