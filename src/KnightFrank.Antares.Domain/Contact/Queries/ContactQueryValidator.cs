namespace KnightFrank.Antares.Domain.Contact.Queries
{
    using System;

    using FluentValidation;

    public class ContactQueryValidator : AbstractValidator<ContactQuery>
    {
        public ContactQueryValidator()
        {
            this.RuleFor(q => q).NotNull();
            this.RuleFor(q => q.Id).NotEqual(Guid.Empty);
        }
    }
}
