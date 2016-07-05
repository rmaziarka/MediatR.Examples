namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using FluentValidation;

    public class UpdateActivityAttendeeValidator : AbstractValidator<UpdateActivityAttendee>
    {
        public UpdateActivityAttendeeValidator()
        {
            this.RuleFor(x => x.UserId).NotEmpty().When(x => !x.ContactId.HasValue);
            this.RuleFor(x => x.UserId).Empty().When(x => x.ContactId.HasValue);
        }
    }
}
