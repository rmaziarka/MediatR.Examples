namespace KnightFrank.Antares.Domain.Common.Validator
{
    using System;

    using FluentValidation;

    public class DateInFutureValidator : AbstractValidator<DateTime?>
    {
        public DateInFutureValidator(string propertyName)
        {
            this.RuleFor(v => v.Value.Date)
                .GreaterThanOrEqualTo(v => DateTime.UtcNow.Date)
                .When(v => v.HasValue)
                .OverridePropertyName(propertyName);
        }
    }
}
