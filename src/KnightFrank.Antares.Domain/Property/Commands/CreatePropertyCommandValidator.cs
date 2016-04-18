namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;

    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly IGenericRepository<PropertyTypeDefinition> propertyTypeDefinitionRepository;

        public CreatePropertyCommandValidator(
            IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository,
            IGenericRepository<AddressForm> addressFormRepository,
            IGenericRepository<PropertyType> propertyTypeRepository,
            IGenericRepository<PropertyTypeDefinition> propertyTypeDefinitionRepository,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.propertyTypeDefinitionRepository = propertyTypeDefinitionRepository;

            this.RuleFor(x => x.Address).NotNull();
            this.RuleFor(x => x.Address).SetValidator(new CreateOrUpdatePropertyAddressValidator(addressFieldDefinitionRepository, addressFormRepository));
            this.RuleFor(v => v.PropertyTypeId).NotEqual(Guid.Empty).NotNull();

            this.RuleFor(x => x.PropertyTypeId).SetValidator(new PropertyTypeValidator(propertyTypeRepository));
            this.RuleFor(x => x.DivisionId).NotEqual(Guid.Empty);
            this.Custom(this.DivisionExists);
            this.Custom(this.PropertyTypeIsValid);
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
    }
}