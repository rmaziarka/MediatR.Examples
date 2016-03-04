namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using FluentValidation;

    public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
    {
        public CreateContactCommandValidator()
        {
            this.RuleFor(x => x.FirstName).NotEmpty().Length(255);
            this.RuleFor(x => x.Surname).NotEmpty().Length(255);
            this.RuleFor(x => x.Title).NotEmpty().Length(255);
        }
    }
}