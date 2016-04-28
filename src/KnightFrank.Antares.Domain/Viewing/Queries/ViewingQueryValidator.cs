namespace KnightFrank.Antares.Domain.Viewing.Queries
{
    using FluentValidation;

    public class ViewingQueryValidator : AbstractValidator<ViewingQuery>
    {
        public ViewingQueryValidator()
        {
            this.RuleFor(q => q).NotNull();
            this.RuleFor(q => q.Id).NotNull();
        }
    }
}
