namespace KnightFrank.Antares.Domain.Property.QueryHandlers
{
    using System.Collections.Generic;
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
                    .GetWithInclude(x => x.PropertyType)
                    .Where(x => x.Division.Code == message.DivisionCode)
                    .Where(x => x.Country.IsoCode == message.CountryCode)
                    .Include(x => x.PropertyType.Parent)
                    .Distinct()
                    .Select(x => x.PropertyType);

            List<PropertyTypeLocalised> propertyTypesLocalised =
                this.propertyTypeLocalisedRepository
                    .GetWithInclude(x => x.PropertyType)
                    .Where(x => x.Locale.IsoCode == message.LocaleCode)
                    .Where(x => propertyTypes.Contains(x.PropertyType)).ToList();

            var result = new PropertyTypeQueryResult
            {
                PropertyTypes = propertyTypesLocalised.Where(x => x.PropertyType.ParentId == null).Select(
                x => new PropertyTypeQuerySingleResult
                {
                    Id = x.PropertyType.Id,
                    ParentId = x.PropertyType.ParentId,
                    Name = x.Value,
                    Children = propertyTypesLocalised.Where(y => y.PropertyType.ParentId == x.PropertyType.Id).Select(
                        y => new PropertyTypeQuerySingleResult
                        {
                            Id = y.PropertyType.Id,
                            ParentId = y.PropertyType.ParentId,
                            Name = y.Value
                        })
                }
                ).ToList()
            };

            if (!result.PropertyTypes.Any())
            {
                throw new DomainValidationException("No configuration found.");
            }

            return result;
        }
    }
}
