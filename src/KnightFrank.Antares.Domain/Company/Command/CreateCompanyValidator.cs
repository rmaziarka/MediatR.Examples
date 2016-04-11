namespace KnightFrank.Antares.Domain.Company.Command
{
    using System;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyValidator()
        {

            this.RuleFor(p => p.Name).NotNull().NotEmpty().Length(1, 128);
            this.RuleFor(p => p.ContactIds).NotEmpty();
        }
    }
}