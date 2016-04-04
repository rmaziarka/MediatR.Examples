namespace KnightFrank.Antares.Domain.Activity.Queries
{
    using FluentValidation;

    public class ActivityQueryValidator : AbstractValidator<ActivityQuery>
    {
        public ActivityQueryValidator()
        {
            this.RuleFor(q => q.Id).NotNull();
        }
    }
}
