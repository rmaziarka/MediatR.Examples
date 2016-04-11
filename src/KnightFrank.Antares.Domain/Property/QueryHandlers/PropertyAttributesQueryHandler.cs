namespace KnightFrank.Antares.Domain.Property.QueryHandlers
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Property.Queries;
    using KnightFrank.Antares.Domain.Property.QueryResults;

    using MediatR;

    public class PropertyAttributesQueryHandler : IRequestHandler<PropertyAttributesQuery, PropertyAttributesQueryResult>
    {
        private readonly IReadGenericRepository<PropertyAttributeFormDefinition> propertyAttributeFormDefinitionRepository;

        public PropertyAttributesQueryHandler(IReadGenericRepository<PropertyAttributeFormDefinition> propertyAttributeFormDefinitionRepository)
        {
            this.propertyAttributeFormDefinitionRepository = propertyAttributeFormDefinitionRepository;
        }

        public PropertyAttributesQueryResult Handle(PropertyAttributesQuery message)
        {
            var singleResults = this.propertyAttributeFormDefinitionRepository
                            .GetWithInclude(x => x.Attribute)
                            .Where(x => x.PropertyAttributeForm.Country.IsoCode == message.CountryCode)
                            .Where(x => x.PropertyAttributeForm.PropertyTypeId == message.PropertyTypeId)
                            .OrderBy(x => x.Order)
                            .Select(x => new PropertyAttributesQuerySingleResult
                            {
                                Order = x.Order,
                                NameKey = x.Attribute.NameKey,
                                LabelKey = x.Attribute.LabelKey
                            }).ToList();

            if (!singleResults.Any())
            {
                throw new DomainValidationException("query", "No configuration found.");
            }

            return new PropertyAttributesQueryResult { Attributes = singleResults };
        }
    }
}
