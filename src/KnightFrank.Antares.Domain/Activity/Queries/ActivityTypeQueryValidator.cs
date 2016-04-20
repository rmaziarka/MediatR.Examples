namespace KnightFrank.Antares.Domain.Activity.Queries
{
    using FluentValidation;

    public class ActivityTypeQueryValidator : AbstractValidator<ActivityTypeQuery>
    {
        public ActivityTypeQueryValidator()
        {
            this.RuleFor(q => q.CountryCode).NotEmpty();
            this.RuleFor(q => q.PropertyTypeId).NotEmpty();
        }
    }
}
