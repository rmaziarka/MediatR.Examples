namespace KnightFrank.Antares.Domain.Resource.Queries
{
    using FluentValidation;

    public class ResourceLocalisedQueryValidator : AbstractValidator<ResourceLocalisedQuery>
    {
        public ResourceLocalisedQueryValidator()
        {
            this.RuleFor(q => q.IsoCode).NotNull().NotEmpty().Length(1, 2);
        }
    }
}
