namespace KnightFrank.Antares.Domain.User.Commands
{
    using FluentValidation;

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            this.RuleFor(v => v.Id).NotEmpty();
            this.RuleFor(v => v.SalutationFormatId).NotEmpty();
        }
    }
}