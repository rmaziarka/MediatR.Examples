namespace KnightFrank.Antares.Domain.Property.QueryHandlers
{
    using System.Data.Entity;
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
            IQueryable<PropertyType> propertyTypes =
                this.propertyTypeDefinitionRepository
                    .Get()
                    .Where(x => x.Division.Code == message.DivisionCode)
                    .Where(x => x.Country.IsoCode == message.CountryCode)
                    .Include(x => x.PropertyType.Parent)
                    .Distinct()
                    .Select(x => x.PropertyType);

            IQueryable<PropertyTypeLocalised> propertyTypesLocalised =
                this.propertyTypeLocalisedRepository
                    .Get()
                    .Where(x => x.Locale.IsoCode == message.LocaleCode)
                    .Where(x => propertyTypes.Contains(x.PropertyType));

            var result = new PropertyTypeQueryResult
            {
                PropertyTypes = propertyTypesLocalised
                    .Select(x =>
                        new PropertyTypeQuerySingleResult
                        {
                            Id = x.PropertyType.Id,
                            ParentId = x.PropertyType.ParentId,
                            Name = x.Value
                        })
                    .ToList()
            };

            if (!result.PropertyTypes.Any())
            {
                throw new DomainValidationException("No configuration found.");
            }

            return result;
        }
    }
}
