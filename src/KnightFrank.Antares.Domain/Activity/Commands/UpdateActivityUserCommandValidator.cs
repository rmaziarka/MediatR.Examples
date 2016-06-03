namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    using FluentValidation;

    public class UpdateActivityUserCommandValidator : AbstractValidator<UpdateActivityUserCommand>
    {
        public UpdateActivityUserCommandValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.ActivityId).NotEmpty();
            this.RuleFor(x => x.CallDate).GreaterThanOrEqualTo(DateTime.UtcNow.Date).When(x => x.CallDate.HasValue);
        }
    }
}
