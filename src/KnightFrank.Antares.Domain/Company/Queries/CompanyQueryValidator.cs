namespace KnightFrank.Antares.Domain.Company.Queries
{
    using FluentValidation;

    public class CompanyQueryValidator : AbstractValidator<CompanyQuery>
    {
        public CompanyQueryValidator()
        {
            this.RuleFor(q => q.Id).NotEmpty();
         }
    }
}
