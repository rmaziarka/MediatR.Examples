namespace KnightFrank.Antares.Domain.UnitTests.Activity.Queries
{
    using System;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Activity.Queries;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("ActivityQueryValidator")]
    [Trait("FeatureTitle", "Activity")]
    public class ActivityQueryValidatorTests
    {
        private const string NotEmptyError = "notempty_error";

        [Theory]
        [AutoData]
        public void Given_CorrectActivityQuery_When_Validating_Then_IsValid(
            ActivityQueryValidator validator,
            ActivityQuery query)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void Given_ActivityIdIsEmpty_When_Validating_Then_IsInvalid(
            ActivityQueryValidator validator,
            ActivityQuery query)
        {
            // Arrange
            query.Id = default(Guid);

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(query.Id));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == NotEmptyError);
        }
    }
}
