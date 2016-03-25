namespace KnightFrank.Antares.Domain.Property.Queries
{
    using FluentValidation;

    public class PropertyTypeQueryValidator:AbstractValidator<PropertyTypeQuery>
    {
        public PropertyTypeQueryValidator()
        {
            this.RuleFor(x => x).NotNull();
            this.RuleFor(x => x.CountryCode).NotEmpty();
            this.RuleFor(x => x.DivisionCode).NotEmpty();
        }
    }
}
