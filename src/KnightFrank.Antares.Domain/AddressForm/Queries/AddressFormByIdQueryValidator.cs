namespace KnightFrank.Antares.Domain.AddressForm.Queries
{
    using System;

    using FluentValidation;

    public class AddressFormByIdQueryValidator : AbstractValidator<AddressFormByIdQuery>
    {
        public AddressFormByIdQueryValidator()
        {
            RuleFor(q => q).NotNull();
            RuleFor(q => q.Id).NotEqual(q => Guid.Empty);
        }
    }
}
