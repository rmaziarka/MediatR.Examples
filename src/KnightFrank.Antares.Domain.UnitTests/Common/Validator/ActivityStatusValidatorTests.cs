namespace KnightFrank.Antares.Domain.UnitTests.Common.Validator
{
    using System;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Common.Validator;

    using Xunit;

    [Trait("FeatureTitle", "Activity")]
    [Collection("ActivityStatusValidator")]
    public class ActivityStatusValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ActivityStatusDoesNotExist_When_Validating_Then_IsInvalid(
           ActivityStatusValidator validator,
           Guid activityStatusId)
        {
            // Act
            ValidationResult validationResult = validator.Validate(activityStatusId);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(activityStatusId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorMessage == "Activity Status does not exist.");
        }
    }
}