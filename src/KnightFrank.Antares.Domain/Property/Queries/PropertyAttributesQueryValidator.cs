namespace KnightFrank.Antares.Domain.Property.Queries
{
    using FluentValidation;

    public class PropertyAttributesQueryValidator : AbstractValidator<PropertyAttributesQuery>
    {
        public PropertyAttributesQueryValidator()
        {
            this.RuleFor(x => x.CountryCode).NotEmpty();
            this.RuleFor(x => x.PropertyTypeId).NotEmpty();
        }
    }
}
