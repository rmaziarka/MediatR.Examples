namespace KnightFrank.Antares.Domain.Enum.Queries
{
    using FluentValidation;

    public class EnumLocalisedQueryValidator : AbstractValidator<EnumLocalisedQuery>
    {
        public EnumLocalisedQueryValidator()
        {
            this.RuleFor(q => q.IsoCode).NotNull().NotEmpty().Length(1, 2);
        }
    }
}
