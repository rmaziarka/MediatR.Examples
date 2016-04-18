namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;

    public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
    {
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public UpdatePropertyCommandValidator(
            IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository,
            IGenericRepository<AddressForm> addressFormRepository,
            IGenericRepository<PropertyType> propertyTypeRepository,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;

            this.RuleFor(v => v.Id).NotNull();
            this.RuleFor(x => x.Address).NotNull();
            this.RuleFor(x => x.Address).SetValidator(new CreateOrUpdatePropertyAddressValidator(addressFieldDefinitionRepository, addressFormRepository));
            this.RuleFor(v => v.PropertyTypeId).NotEqual(Guid.Empty).NotNull();

            this.RuleFor(x => x.PropertyTypeId).SetValidator(new PropertyTypeValidator(propertyTypeRepository));
            this.RuleFor(x => x.DivisionId).NotEqual(Guid.Empty);
            this.Custom(this.DivisionExists);
        }
        
        private ValidationFailure DivisionExists(UpdatePropertyCommand updatePropertyCommand)
        {
            bool divisionExists = updatePropertyCommand != null && this.enumTypeItemRepository.Any(x => x.Id.Equals(updatePropertyCommand.DivisionId));
            return divisionExists
                ? null
                : new ValidationFailure(nameof(updatePropertyCommand.DivisionId), "Division does not exist.");
        }
    }
}