namespace KnightFrank.Antares.Domain.Viewing.Commands
{
    using System;

    using FluentValidation;

    public class CreateViewingCommandValidator : AbstractValidator<CreateViewingCommand>
    {
        public CreateViewingCommandValidator()
        {
            this.RuleFor(x => x.StartDate).NotEmpty();
            this.RuleFor(x => x.EndDate).NotEmpty();

            this.RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate)
                .When(x => x.StartDate != DateTime.MinValue)
                .When(x => x.EndDate != DateTime.MinValue)
                .OverridePropertyName("EndDate")
                .WithMessage("End date cannot be earlier than start date.");

            this.RuleFor(x => x.StartDate.Date).Equal(x => x.EndDate.Date)
                .When(x => x.StartDate != DateTime.MinValue)
                .When(x => x.EndDate != DateTime.MinValue)
                .OverridePropertyName("EndDate")
                .WithMessage("Start date and end date must be on the same day.");

            this.RuleFor(x => x.ActivityId).NotEmpty();
            this.RuleFor(x => x.RequirementId).NotEmpty();
            this.RuleFor(x => x.InvitationText).Length(0, 4000);
            this.RuleFor(x => x.PostViewingComment).Length(0, 4000);
            this.RuleFor(x => x.AttendeesIds).NotEmpty();
        }
    }
}
