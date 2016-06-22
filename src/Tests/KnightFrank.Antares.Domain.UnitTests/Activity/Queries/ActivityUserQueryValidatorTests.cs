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

    [Collection("ActivityUserQueryValidator")]
    [Trait("FeatureTitle", "Activity")]
    public class ActivityUserQueryValidatorTests
    {
        [Theory]
        [AutoData]
        public void Given_CorrectActivityQuery_When_Validating_Then_IsValid(
            ActivityUserQueryValidator validator,
            ActivityUserQuery query)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void Given_IdIsEmpty_When_Validating_Then_IsInvalid(ActivityUserQueryValidator validator, ActivityUserQuery query)
        {
            // Arrange
            query.Id = Guid.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsInvalid(nameof(query.Id), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoData]
        public void Given_ActivityIdIsEmpty_When_Validating_Then_IsInvalid(
            ActivityUserQueryValidator validator,
            ActivityUserQuery query)
        {
            // Arrange
            query.ActivityId = Guid.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsInvalid(nameof(query.ActivityId), nameof(Messages.notempty_error));
        }
    }
}
