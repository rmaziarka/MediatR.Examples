namespace KnightFrank.Antares.Domain.Activity.Queries
{
    using FluentValidation;

    public class ActivityUserQueryValidator : AbstractValidator<ActivityUserQuery>
    {
        public ActivityUserQueryValidator()
        {
            this.RuleFor(q => q.Id).NotEmpty();
            this.RuleFor(q => q.ActivityId).NotEmpty();
        }
    }
}
