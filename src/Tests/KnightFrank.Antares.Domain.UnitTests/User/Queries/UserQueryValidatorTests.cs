namespace KnightFrank.Antares.Domain.UnitTests.User.Queries
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.User.Queries;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("UserQueryValidator")]
    [Trait("FeatureTitle", "User")]
    public class UserQueryValidatorTests
    {
        [Theory]
        [AutoData]
        public void Given_CorrectInput_When_Validating_Then_NoValidationErrors(
            UserQueryValidator validator,
            UserQuery query)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void Given_EmptyGuidInput_When_Validating_Then_ValidationError(
            UserQueryValidator validator,
            UserQuery query)
        {
            // Setup
            query.Id = Guid.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(query.Id));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == nameof(Messages.notempty_error));
        }     
    }
}