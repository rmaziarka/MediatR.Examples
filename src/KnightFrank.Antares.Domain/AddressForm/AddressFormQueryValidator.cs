namespace KnightFrank.Antares.Domain.AddressForm
{
    using FluentValidation;

    public class AddressFormQueryValidator : AbstractValidator<AddressFormQuery>
    {
        public AddressFormQueryValidator()
        {
            this.RuleFor(q => q).NotNull();
            this.RuleFor(q => q.EntityType).NotEmpty();
            this.RuleFor(q => q.CountryCode).NotEmpty();
        }
    }
}
