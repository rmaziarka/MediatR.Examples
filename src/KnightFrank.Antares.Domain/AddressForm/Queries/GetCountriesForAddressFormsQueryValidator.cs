namespace KnightFrank.Antares.Domain.AddressForm.Queries
{
    using FluentValidation;

    public class GetCountriesForAddressFormsQueryValidator : AbstractValidator<GetCountriesForAddressFormsQuery>
    {
        public GetCountriesForAddressFormsQueryValidator()
        {
            this.RuleFor(x => x).NotNull();
            this.RuleFor(x => x.EntityTypeItemCode).NotEmpty();
        }
    }
}