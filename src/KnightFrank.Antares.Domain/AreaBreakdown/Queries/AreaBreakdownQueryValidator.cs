namespace KnightFrank.Antares.Domain.AreaBreakdown.Queries
{
    using FluentValidation;
    public class AreaBreakdownQueryValidator : AbstractValidator<AreaBreakdownQuery>
    {
        public AreaBreakdownQueryValidator()
        {
            this.RuleFor(x => x.PropertyId).NotEmpty();
        }
    }
}