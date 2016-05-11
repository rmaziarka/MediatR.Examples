namespace KnightFrank.Antares.Domain.Viewing.Commands
{
    using System;

    using FluentValidation;

    public class UpdateViewingCommandValidator : AbstractValidator<UpdateViewingCommand>
    {
        public UpdateViewingCommandValidator()
        {
            this.RuleFor(x => x.StartDate).NotEmpty();
            this.RuleFor(x => x.EndDate).NotEmpty();

            this.RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate)
                .When(x => x.StartDate != DateTime.MinValue)
                .When(x => x.EndDate != DateTime.MinValue)
                .OverridePropertyName("EndDate")
                .WithMessage("End date cannot be earlier than start date.");

            this.RuleFor(x => x.InvitationText).Length(0, 4000);
            this.RuleFor(x => x.PostViewingComment).Length(0, 4000);
            this.RuleFor(x => x.AttendeesIds).NotEmpty();
        }
    }
}
