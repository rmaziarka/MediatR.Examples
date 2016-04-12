﻿namespace KnightFrank.Antares.Domain.Property.Commands
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

        public CreatePropertyCommandValidator(
            IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository,
            IGenericRepository<AddressForm> addressFormRepository,
            IGenericRepository<PropertyType> propertyTypeRepository,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;

            this.RuleFor(x => x.Address).NotNull();
            this.RuleFor(x => x.Address).SetValidator(new CreateOrUpdatePropertyAddressValidator(addressFieldDefinitionRepository, addressFormRepository));
            this.RuleFor(v => v.PropertyTypeId).NotEqual(Guid.Empty).NotNull();

            this.RuleFor(x => x.PropertyTypeId).SetValidator(new PropertyTypeValidator(propertyTypeRepository));
            this.RuleFor(x => x.Division).NotNull();
            this.RuleFor(x => x.Division.Code).NotEmpty().When(x => x.Division != null);
            this.Custom(this.DivisionCodeExists);
        }

        private ValidationFailure DivisionCodeExists(CreatePropertyCommand createPropertyCommand)
        {
            var divisionExists = createPropertyCommand != null && this.enumTypeItemRepository.Any(x => x.Code.Equals(createPropertyCommand.Division.Code));
            return divisionExists
                ? null
                : new ValidationFailure(nameof(createPropertyCommand.Division), "Division does not exist.");
        }
    }
}