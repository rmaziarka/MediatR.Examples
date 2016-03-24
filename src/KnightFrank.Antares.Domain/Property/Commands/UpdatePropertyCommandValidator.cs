namespace KnightFrank.Antares.Domain.Property.Commands
{
    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Repository;

    public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyCommandValidator(
            IGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository,
            IGenericRepository<AddressForm> addressFormRepository)
        {
            this.RuleFor(v => v.Id).NotNull();
            this.RuleFor(x => x.Address).SetValidator(new CreateOrUpdatePropertyAddressValidator(addressFieldDefinitionRepository, addressFormRepository));
        }
    }
}