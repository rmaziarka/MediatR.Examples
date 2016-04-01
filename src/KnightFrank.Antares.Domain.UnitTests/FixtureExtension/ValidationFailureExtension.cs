using System.Collections.Generic;

namespace KnightFrank.Antares.Domain.UnitTests.FixtureExtension
{
    using FluentValidation.Results;

    using Ploeh.AutoFixture;

    public static class ValidationFailureExtension
    {
        public static ValidationResult BuildValidationResult(this IFixture fixture, string propertyName = null, string error = null)
        {
            return new ValidationResult(new List<ValidationFailure>() {new ValidationFailure(propertyName, error)});
        }
    }
}
