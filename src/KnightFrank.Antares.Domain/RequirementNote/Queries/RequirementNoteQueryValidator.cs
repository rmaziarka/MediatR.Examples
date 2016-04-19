namespace KnightFrank.Antares.Domain.RequirementNote.Queries
{
    using FluentValidation;

    public class RequirementNoteQueryValidator : AbstractValidator<RequirementNoteQuery>
    {
        public RequirementNoteQueryValidator()
        {
            this.RuleFor(q => q.Id).NotEmpty();
        }
    }
}
