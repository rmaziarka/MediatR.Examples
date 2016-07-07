namespace KnightFrank.Antares.Domain.Common.Validator
{
    using System;

    using FluentValidation;

    public class DateInPastValidator : AbstractValidator<DateTime>
    {
        public DateInPastValidator(string propertyName)
        {
            this.RuleFor(x => x.Date)
                .LessThanOrEqualTo(x => DateTime.UtcNow.Date)
                .WithName(propertyName);
        }
    }
}
