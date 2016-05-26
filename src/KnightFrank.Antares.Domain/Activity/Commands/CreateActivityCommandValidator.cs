namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using FluentValidation;

    public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {
        public CreateActivityCommandValidator()
        {
            this.RuleFor(x => x.PropertyId).NotEmpty();

            this.RuleFor(x => x.ActivityStatusId).NotEmpty();

            this.RuleFor(x => x.ActivityTypeId).NotEmpty();

            this.RuleFor(x => x.LeadNegotiatorId).NotEmpty();
        }
    }
}
