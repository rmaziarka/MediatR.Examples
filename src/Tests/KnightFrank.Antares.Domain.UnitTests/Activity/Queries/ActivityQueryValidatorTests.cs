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

    [Collection("ActivityQueryValidator")]
    [Trait("FeatureTitle", "Activity")]
    public class ActivityQueryValidatorTests
    {
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
            validationResult.IsInvalid(nameof(query.Id), nameof(Messages.notempty_error));
        }
    }
}
