namespace KnightFrank.Antares.Domain.Property.Queries
{
    using FluentValidation;

    public class PropertyQueryValidator : AbstractValidator<PropertyQuery>
    {
        public PropertyQueryValidator()
        {
            this.RuleFor(q => q).NotNull();
            this.RuleFor(q => q.Id).NotNull();
        }
    }
}