namespace KnightFrank.Antares.Domain.LatestView.Commands
{
    using FluentValidation;

    public class CreateLatestViewCommandValidator : AbstractValidator<CreateLatestViewCommand>
    {
        public CreateLatestViewCommandValidator()
        {
            this.RuleFor(x => x.EntityType).NotEmpty();
            this.RuleFor(x => x.EntityId).NotEmpty();
        }
    }
}
