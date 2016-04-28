namespace KnightFrank.Antares.Domain.Viewing.Commands
{
    using FluentValidation;

    public class CreateViewingCommandValidator : AbstractValidator<CreateViewingCommand>
    {
        public CreateViewingCommandValidator()
        {
            this.RuleFor(x => x.StartDate).NotEmpty();
            this.RuleFor(x => x.EndDate).NotEmpty();
            this.RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
            this.RuleFor(x => x.StartDate.Date).Equal(x => x.EndDate.Date);
            this.RuleFor(x => x.ActivityId).NotEmpty();
            this.RuleFor(x => x.RequirementId).NotEmpty();
            this.RuleFor(x => x.InvitationText).Length(0, 4000);
            this.RuleFor(x => x.PostViewingComment).Length(0, 4000);
            this.RuleFor(x => x.AttendeesIds).NotEmpty();
        }
    }
}
