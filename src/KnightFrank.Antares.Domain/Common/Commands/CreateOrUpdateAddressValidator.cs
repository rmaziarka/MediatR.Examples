namespace KnightFrank.Antares.Domain.Common.Commands
{
    using System;

    using FluentValidation;

    public class CreateOrUpdateAddressValidator : AbstractValidator<CreateOrUpdateAddress>
    {
        public CreateOrUpdateAddressValidator()
        {
            this.RuleFor(x => x.CountryId).NotEqual(Guid.Empty);
            this.RuleFor(x => x.AddressFormId).NotEqual(Guid.Empty).NotNull();
        }
    }
}