namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using FluentValidation;

    public class UpdateActivityUserCommandValidator : AbstractValidator<UpdateActivityUserCommand>
    {
        public UpdateActivityUserCommandValidator(
)
        {
            this.RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
