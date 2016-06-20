namespace KnightFrank.Antares.Domain.UnitTests.RequirementNote.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.RequirementNote.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Ploeh.AutoFixture;

    using Xunit;

    [Trait("FeatureTitle", "RequirementNote")]
    [Collection("CreateRequirementNoteCommandValidator")]
    public class CreateRequirementNoteCommandValidatorTests : IClassFixture<CreateRequirementNoteCommand>
    {
        [Theory]
        [AutoMoqData]
        [InlineAutoMoqData(null)]
        [InlineAutoMoqData("")]
        [InlineAutoMoqData("a")]
        [InlineAutoMoqData("abc")]
        public void Given_ValidCreateRequirementNoteCommand_When_Validating_Then_IsValid(
            string description,
            CreateRequirementNoteCommandValidator validator,
            CreateRequirementNoteCommand cmd)
        {
            // Arrange

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_DescriptionWithMaxLength_When_Validating_Then_IsValid(
            CreateRequirementNoteCommandValidator validator,
            CreateRequirementNoteCommand cmd)
        {
            // Arrange
            cmd.Description = string.Join(string.Empty, new Fixture().CreateMany<char>(4000));

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_DescriptionGreaterThanMaxLength_When_Validating_Then_IsInvalid(
            CreateRequirementNoteCommandValidator validator,
            CreateRequirementNoteCommand cmd)
        {
            // Arrange
            cmd.Description = string.Join(string.Empty, new Fixture().CreateMany<char>(4001));

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsInvalid(nameof(cmd.Description), nameof(Messages.length_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandRequirementIdIsEmpty_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode(
            CreateRequirementNoteCommandValidator validator,
            CreateRequirementNoteCommand cmd)
        {
            // Arrange
            cmd.RequirementId = default(Guid);

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsInvalid(nameof(cmd.RequirementId), nameof(Messages.notempty_error));
        }
    }
}
