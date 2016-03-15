namespace KnightFrank.Antares.Domain.Requirement
{
    using FluentValidation;

    public class RequirementQueryValidator : AbstractValidator<RequirementQuery>
    {
        public RequirementQueryValidator()
        {
            this.RuleFor(q => q).NotNull();
            this.RuleFor(q => q.Id).NotNull();
        }
    }
}
