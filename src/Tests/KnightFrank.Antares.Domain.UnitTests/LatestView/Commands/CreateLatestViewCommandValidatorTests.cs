namespace KnightFrank.Antares.Domain.UnitTests.LatestView.Commands
{
    using System;

    using FluentAssertions;
    
    using FluentValidation.Resources;
    using FluentValidation.Results;

    using Dal.Model.Common;
    using Domain.LatestView.Commands;

    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Xunit;

    [Trait("FeatureTitle", "LatestView")]
    [Collection("CreateLatestViewCommandValidator")]
    public class CreateLatestViewCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCreateLatestViewCommand_When_Validating_Then_IsValid(
             CreateLatestViewCommandValidator validator,
             CreateLatestViewCommand cmd)
        {
            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyEntityId_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
             CreateLatestViewCommandValidator validator,
             CreateLatestViewCommand cmd)
        {
            // Arrange
            cmd.EntityId = Guid.Empty;

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.EntityId));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvalidEntityType_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
             CreateLatestViewCommandValidator validator,
             CreateLatestViewCommand cmd)
        {
            // Arrange
            cmd.EntityType = (EntityTypeEnum)123;

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.EntityType));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.enum_error));
        }
    }
}
