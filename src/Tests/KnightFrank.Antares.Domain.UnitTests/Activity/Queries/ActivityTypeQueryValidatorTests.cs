namespace KnightFrank.Antares.Domain.UnitTests.Activity.Queries
{
    using System;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Activity.Queries;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("ActivityTypeQueryValidator")]
    [Trait("FeatureTitle", "Activity")]
    public class ActivityTypeQueryValidatorTests
    {
        private const string NotEmptyError = "notempty_error";
        
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
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(query.CountryCode));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == NotEmptyError);
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
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(query.PropertyTypeId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == NotEmptyError);
        }
    }
}
