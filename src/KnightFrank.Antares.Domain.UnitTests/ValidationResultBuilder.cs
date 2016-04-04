using System.Collections.Generic;

namespace KnightFrank.Antares.Domain.UnitTests
{
    using FluentValidation.Results;

    public static class ValidationResultBuilder
    {
        public static ValidationResult BuildValidationResult(string propertyName = null, string error = null)
        {
            return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure(propertyName, error) });
        }
    }
}
