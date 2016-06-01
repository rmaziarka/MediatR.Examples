namespace KnightFrank.Antares.Domain.Contact.Queries
{
    using System;

    using FluentValidation;

    public class CompanyQueryValidator : AbstractValidator<CompanyQuery>
    {
        public CompanyQueryValidator()
        {
            this.RuleFor(q => q).NotNull();
            this.RuleFor(q => q.Id).NotEqual(Guid.Empty);
        }
    }
}
