namespace KnightFrank.Antares.Domain.Property.Commands
{
    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;

    public class PropertyDomainValidator : AbstractValidator<Property>, IDomainValidator<Property>
    {
        private readonly IGenericRepository<PropertyTypeDefinition> propertyTypeDefinitionRepository;

        public PropertyDomainValidator(IGenericRepository<PropertyTypeDefinition> propertyTypeDefinitionRepository)
        {
            this.propertyTypeDefinitionRepository = propertyTypeDefinitionRepository;
            
            this.Custom(this.PropertyTypeIsValid);
        }
        
        public ValidationFailure PropertyTypeIsValid(Property property)
        {
            bool propertyTypeDefinitionExist =
                this.propertyTypeDefinitionRepository.Any(p => p.PropertyTypeId.Equals(property.PropertyTypeId)
                                                                  && p.DivisionId.Equals(property.DivisionId)
                                                                  && p.CountryId.Equals(property.Address.CountryId));
            if(!propertyTypeDefinitionExist)
            {
                return new ValidationFailure(nameof(property.PropertyTypeId), "Specified property type is invalid");
            }

            return null;
        }
    }
}