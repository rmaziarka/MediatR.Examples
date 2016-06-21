namespace KnightFrank.Antares.Domain.UnitTests.AreaBreakdown.Commands
{
    using System;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "AreaBreakdown")]
    [Collection("UpdateAreaBreakdownCommandValidator")]
    public class UpdateAreaBreakdownCommandValidatorTests
    {
        [Theory]
        [AutoData]
        public void Given_UpdateAreaBreakdownCommand_When_Validating_Then_NoValidationErrors(
            UpdateAreaBreakdownCommand command,
            UpdateAreaBreakdownCommandValidator validator)
        {
            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoData]
        public void Given_EmptyId_When_Validating_Then_ValidationErrors(
            UpdateAreaBreakdownCommand command,
            UpdateAreaBreakdownCommandValidator validator)
        {
            // Arrange
            command.Id = Guid.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.Id), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoData]
        public void Given_EmptyPropertyId_When_Validating_Then_ValidationErrors(
            UpdateAreaBreakdownCommand command,
            UpdateAreaBreakdownCommandValidator validator)
        {
            // Arrange
            command.PropertyId = Guid.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.PropertyId), nameof(Messages.notempty_error));
        }

        [Theory]
        [InlineAutoData((string)null)]
        [InlineAutoData("")]
        [InlineAutoData(" ")]
        public void Given_EmptyName_When_Validating_Then_ValidationErrors(
            string name,
            UpdateAreaBreakdownCommand command,
            UpdateAreaBreakdownCommandValidator validator)
        {
            // Arrange
            command.Name = name;

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.Name), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoData]
        public void Given_TooLongName_When_Validating_Then_ValidationErrors(
            UpdateAreaBreakdownCommand command,
            UpdateAreaBreakdownCommandValidator validator,
            IFixture fixture)
        {
            // Arrange
            command.Name = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.Name), nameof(Messages.length_error));
        }

        [Theory]
        [InlineAutoData(-0.1)]
        [InlineAutoData(0.0)]
        public void Given_SmallerThanZeroSize_When_Validating_Then_ValidationErrors(
            double size,
            UpdateAreaBreakdownCommand command,
            UpdateAreaBreakdownCommandValidator validator)
        {
            // Arrange
            command.Size = size;

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.Size), nameof(Messages.greaterthan_error));
        }
    }
}
