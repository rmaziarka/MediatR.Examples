namespace KnightFrank.Antares.Domain.Tenancy.Queries
{
    using FluentValidation;
    public class TenancyQueryValidator : AbstractValidator<TenancyQuery>
    {
        public TenancyQueryValidator()
        {
            this.RuleFor(q => q.Id).NotEmpty();
        }
    }
}
