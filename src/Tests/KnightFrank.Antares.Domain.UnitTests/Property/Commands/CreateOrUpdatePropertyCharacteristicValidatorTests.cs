namespace KnightFrank.Antares.Domain.UnitTests.Property.Commands
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Property.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Xunit;

    [Collection("CreateOrUpdatePropertyCharacteristicValidator")]
    [Trait("FeatureTitle", "Property")]
    public class CreateOrUpdatePropertyCharacteristicValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCreateOrUpdatePropertyCharacteristic_When_Validating_Then_IsValid(
            CreateOrUpdatePropertyCharacteristicValidator validator,
            CreateOrUpdatePropertyCharacteristic cmd)
        {
            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyCharacteristicId_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateOrUpdatePropertyCharacteristicValidator validator,
            CreateOrUpdatePropertyCharacteristic cmd)
        {
            // Arrange
            cmd.CharacteristicId = Guid.Empty;

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.CharacteristicId));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.notempty_error));
        }

        [Theory]
        [InlineAutoMoqData((string)null)]
        [InlineAutoMoqData("")]
        [InlineAutoMoqData(" ")]
        [InlineAutoMoqData("text length to 50 signs 56789012345678901234567890")]
        public void Given_ValidText_When_Validating_Then_IsValid(
            string text,
            CreateOrUpdatePropertyCharacteristicValidator validator,
            CreateOrUpdatePropertyCharacteristic cmd)
        {
            // Arrange
            cmd.Text = text;

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_TextWithOver50Signs_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateOrUpdatePropertyCharacteristicValidator validator,
            CreateOrUpdatePropertyCharacteristic cmd)
        {
            // Arrange
            cmd.Text = string.Join("", Enumerable.Repeat("x",51));

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.Text));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.length_error));
        }
    }
}
