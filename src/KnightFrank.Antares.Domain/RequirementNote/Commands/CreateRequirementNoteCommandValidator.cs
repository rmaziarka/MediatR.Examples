namespace KnightFrank.Antares.Domain.RequirementNote.Commands
{
    using FluentValidation;

    public class CreateRequirementNoteCommandValidator : AbstractValidator<CreateRequirementNoteCommand>
    {
        public CreateRequirementNoteCommandValidator()
        {
            this.RuleFor(x => x.Description).Length(0, 4000);
            this.RuleFor(x => x.RequirementId).NotEmpty();
        }
    }
}