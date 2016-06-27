namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using FluentValidation;

    public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
    {
        public UpdateContactCommandValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.FirstName).NotEmpty().Length(1, 255);
            this.RuleFor(x => x.LastName).NotEmpty().Length(1, 255);
            this.RuleFor(x => x.Title).NotEmpty().Length(1, 255);
        }
    }
}