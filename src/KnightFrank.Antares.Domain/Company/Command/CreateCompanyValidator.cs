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
            Func<CreateCompanyCommand, ValidationFailure> areContactsProvided = cmd =>
            {
                if (cmd.ContactIds != null)
                {
                    return cmd.ContactIds.Any(c => c.Equals(Guid.Empty))
                        ? new ValidationFailure(nameof(cmd.ContactIds), "Contacts are invalid.")
                        {
                            ErrorCode = "contactsempty_error"
                        }
                        : null;
                }

                return null;
            };

            this.RuleFor(p => p.Name).NotNull().NotEmpty().Length(1, 128);
            this.RuleFor(p => p.ContactIds).NotNull().NotEmpty();
            this.Custom(areContactsProvided);
        }
    }
}