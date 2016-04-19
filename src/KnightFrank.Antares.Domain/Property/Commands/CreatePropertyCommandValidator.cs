namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;

    using Attribute = KnightFrank.Antares.Dal.Model.Attribute.Attribute;

    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly IGenericRepository<PropertyTypeDefinition> propertyTypeDefinitionRepository;
        private readonly IGenericRepository<PropertyAttributeForm> propertyAttributeFormRepository;

        public CreatePropertyCommandValidator(
            IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository,
            IGenericRepository<AddressForm> addressFormRepository,
            IGenericRepository<PropertyType> propertyTypeRepository,
            IGenericRepository<PropertyTypeDefinition> propertyTypeDefinitionRepository,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository,
            IGenericRepository<PropertyAttributeForm> propertyAttributeFormRepository)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.propertyTypeDefinitionRepository = propertyTypeDefinitionRepository;
            this.propertyAttributeFormRepository = propertyAttributeFormRepository;

            this.RuleFor(x => x.Address).NotNull();
            this.RuleFor(x => x.Address).SetValidator(new CreateOrUpdatePropertyAddressValidator(addressFieldDefinitionRepository, addressFormRepository));
            this.RuleFor(v => v.PropertyTypeId).NotEqual(Guid.Empty).NotNull();
           
            this.RuleFor(x => x.PropertyTypeId).SetValidator(new PropertyTypeValidator(propertyTypeRepository));
            this.RuleFor(x => x.DivisionId).NotEqual(Guid.Empty);

            this.Custom(this.DivisionExists);
            this.Custom(this.PropertyTypeIsValid);
            this.Custom(this.AttributeValuesAreAllowedForPropertyType);
        }

        private ValidationFailure DivisionExists(CreatePropertyCommand createPropertyCommand)
        {
            bool divisionExists = createPropertyCommand != null && this.enumTypeItemRepository.Any(x => x.Id.Equals(createPropertyCommand.DivisionId));
            return divisionExists
                ? null
                : new ValidationFailure(nameof(createPropertyCommand.DivisionId), "Division does not exist.");
        }

        public ValidationFailure PropertyTypeIsValid(CreatePropertyCommand createPropertyCommand)
        {
            bool propertyTypeDefinitionExist =
                this.propertyTypeDefinitionRepository.Any(p => p.PropertyTypeId.Equals(createPropertyCommand.PropertyTypeId)
                                                                  && p.DivisionId.Equals(createPropertyCommand.DivisionId)
                                                                  && p.CountryId.Equals(createPropertyCommand.Address.CountryId));
            return !propertyTypeDefinitionExist ? new ValidationFailure(nameof(createPropertyCommand.PropertyTypeId), "Specified property type is invalid") : null;
        }

        private ValidationFailure AttributeValuesAreAllowedForPropertyType(CreatePropertyCommand updatePropertyCommand)
        {
            PropertyAttributeForm propertyAttributeForm = this.propertyAttributeFormRepository
                .GetWithInclude(p => p.PropertyTypeId == updatePropertyCommand.PropertyTypeId && p.Country.Id == updatePropertyCommand.Address.CountryId,
                    p => p.PropertyAttributeFormDefinitions.Select(s => s.Attribute))
                .SingleOrDefault();

            if (propertyAttributeForm == null)
                return null;

            IEnumerable<Attribute> allowedAttributes = propertyAttributeForm
                .PropertyAttributeFormDefinitions
                .Select(s => s.Attribute);

            List<string> allowedAttributeNames = allowedAttributes.Select(a => a.NameKey).ToList();

            PropertyInfo[] attributeValueProperties = typeof(CreateOrUpdatePropertyAttributeValues)
                .GetProperties();

            List<string> attributeValueNames = attributeValueProperties.Select(a => a.Name).ToList();
            
            IEnumerable<string> allowedAttributeValues =
                from attributeValueName in attributeValueNames
                from allowedAttributeName in allowedAttributeNames
                where attributeValueName == "Min" + allowedAttributeName 
                   || attributeValueName == "Max" + allowedAttributeName
                select attributeValueName;

            IEnumerable<object> notAllowedAttributeValues = attributeValueProperties
                .Where(propertyInfo => !allowedAttributeValues.Contains(propertyInfo.Name))
                .Select(propertyInfo => propertyInfo.GetValue(updatePropertyCommand.AttributeValues));

            bool allNotAllowedAttributeValuesAreNull = notAllowedAttributeValues.All(value => value == null);

            return allNotAllowedAttributeValuesAreNull
                ? null
                : new ValidationFailure(nameof(updatePropertyCommand.AttributeValues), "Invalid attributes values are defined.");
        }
    }
}