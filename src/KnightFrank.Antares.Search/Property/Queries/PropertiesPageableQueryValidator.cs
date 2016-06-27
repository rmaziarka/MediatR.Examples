namespace KnightFrank.Antares.Search.Property.Queries
{
    using FluentValidation;

    public class PropertiesPageableQueryValidator : AbstractValidator<PropertiesPageableQuery>
    {
        public PropertiesPageableQueryValidator()
        {
            this.RuleFor(x => x.Page).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.Size).GreaterThan(0);
            this.RuleFor(x => x.Query).NotEmpty();
        }
    }
}
