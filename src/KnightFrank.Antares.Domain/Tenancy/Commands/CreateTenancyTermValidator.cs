namespace KnightFrank.Antares.Domain.Tenancy.Commands
{
    using FluentValidation;

    public class CreateTenancyTermValidator : AbstractValidator<UpdateTenancyTerm>
    {
        public CreateTenancyTermValidator()
        {
            this.RuleFor(x => x.AgreedRent).NotEmpty();
            this.RuleFor(x => x.StartDate).NotEmpty();
            this.RuleFor(x => x.EndDate).NotEmpty();
            this.RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        }
    }
}
