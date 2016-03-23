namespace KnightFrank.Antares.Domain.Property.Commands
{
    using FluentValidation;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;

    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator(IGenericRepository<AddressFieldDefinition> addressFieldDefinition)
        {
            this.RuleFor(x => x.Address).SetValidator(new CreateOrUpdatePropertyAddressValidator(addressFieldDefinition));
        }
    }
}