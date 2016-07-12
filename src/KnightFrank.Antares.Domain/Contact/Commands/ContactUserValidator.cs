namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using FluentValidation;

    public class ContactUserValidator: AbstractValidator<ContactUserCommand>
    {
        public ContactUserValidator()
        {
            this.RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
