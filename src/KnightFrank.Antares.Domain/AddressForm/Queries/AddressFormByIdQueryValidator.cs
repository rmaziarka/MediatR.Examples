namespace KnightFrank.Antares.Domain.AddressForm.Queries
{
    using System;

    using FluentValidation;

    public class AddressFormByIdQueryValidator : AbstractValidator<AddressFormByIdQuery>
    {
        public AddressFormByIdQueryValidator()
        {
            this.RuleFor(q => q).NotNull();
            this.RuleFor(q => q.Id).NotEqual(q => Guid.Empty);
        }
    }
}
