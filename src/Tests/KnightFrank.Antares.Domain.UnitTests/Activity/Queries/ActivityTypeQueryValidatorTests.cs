namespace KnightFrank.Antares.Domain.UnitTests.Activity.Queries
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Activity.Queries;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("ActivityTypeQueryValidator")]
    [Trait("FeatureTitle", "Activity")]
    public class ActivityTypeQueryValidatorTests
    {
        [Theory]
        [AutoData]
        public void Given_CorrectActivityTypeQuery_When_Validating_Then_IsValid(
            ActivityTypeQueryValidator validator,
            ActivityTypeQuery query)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void Given_CountryIsEmpty_When_Validating_Then_IsInvalid(
            ActivityTypeQueryValidator validator,
            ActivityTypeQuery query)
        {
            // Arrange
            query.CountryCode = String.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsInvalid(nameof(query.CountryCode), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoData]
        public void Given_PropertyTypeIdIsEmpty_When_Validating_Then_IsInvalid(
            ActivityTypeQueryValidator validator,
            ActivityTypeQuery query)
        {
            // Arrange
            query.PropertyTypeId = Guid.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsInvalid(nameof(query.PropertyTypeId), nameof(Messages.notempty_error));
        }
    }
}
