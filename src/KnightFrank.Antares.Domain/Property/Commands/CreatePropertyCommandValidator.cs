namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;

    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;

    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator(
            IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository,
            IGenericRepository<AddressForm> addressFormRepository,
            IGenericRepository<PropertyType> propertyTypeRepository)
        {
            this.RuleFor(x => x.Address).NotNull();
            this.RuleFor(x => x.Address).SetValidator(new CreateOrUpdatePropertyAddressValidator(addressFieldDefinitionRepository, addressFormRepository));
            this.RuleFor(v => v.PropertyTypeId).NotEqual(Guid.Empty).NotNull();

            this.RuleFor(x => x.PropertyTypeId).SetValidator(new PropertyTypeValidator(propertyTypeRepository));
        }
    }
}