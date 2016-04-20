namespace KnightFrank.Antares.Domain.Property.QueryHandlers
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Property.Queries;
    using KnightFrank.Antares.Domain.Property.QueryResults;

    using MediatR;

    public class PropertyTypeQueryHandler : IRequestHandler<PropertyTypeQuery, PropertyTypeQueryResult>
    {
        private readonly IReadGenericRepository<PropertyTypeDefinition> propertyTypeDefinitionRepository;
        private readonly IReadGenericRepository<PropertyTypeLocalised> propertyTypeLocalisedRepository;

        public PropertyTypeQueryHandler(IReadGenericRepository<PropertyTypeDefinition> propertyTypeDefinitionRepository, IReadGenericRepository<PropertyTypeLocalised> propertyTypeLocalisedRepository)
        {
            this.propertyTypeDefinitionRepository = propertyTypeDefinitionRepository;
            this.propertyTypeLocalisedRepository = propertyTypeLocalisedRepository;
        }

        public PropertyTypeQueryResult Handle(PropertyTypeQuery message)
        {
            var items = (from d in this.propertyTypeDefinitionRepository.GetWithInclude(x => x.PropertyType)
                         where d.Division.Code == message.DivisionCode
                         where d.Country.IsoCode == message.CountryCode
                         join l in this.propertyTypeLocalisedRepository.Get() on d.PropertyTypeId equals l.ResourceId
                         where l.Locale.IsoCode == message.LocaleCode
                         orderby d.Order
                         select new { d.Order, d.PropertyType, l.Value }).ToList();
            
            var result = new PropertyTypeQueryResult
            {
                PropertyTypes = items.Where(x => x.PropertyType.ParentId == null).Select(
                x => new PropertyTypeQuerySingleResult
                {
                    Id = x.PropertyType.Id,
                    ParentId = x.PropertyType.ParentId,
                    Name = x.Value,
                    Order = x.Order,
                    Children = items.Where(y => y.PropertyType.ParentId == x.PropertyType.Id).Select(
                        y => new PropertyTypeQuerySingleResult
                        {
                            Id = y.PropertyType.Id,
                            ParentId = y.PropertyType.ParentId,
                            Name = y.Value,
                            Order = y.Order
                        })
                }
                ).ToList()
            };

            if (!result.PropertyTypes.Any())
            {
                throw new DomainValidationException("query", "No configuration found.");
            }

            return result;
        }
    }
}
