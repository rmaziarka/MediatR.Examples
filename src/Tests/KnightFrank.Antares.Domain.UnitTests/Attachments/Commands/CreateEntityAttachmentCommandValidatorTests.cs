namespace KnightFrank.Antares.Domain.UnitTests.Attachments.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Domain.Attachment.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Xunit;

    [Trait("FeatureTitle", "Property Entity")]
    [Collection("CreateEntityAttachment")]
    public class CreateEntityAttachmentCommandValidatorTests
    {
        [Theory]
        [AutoConfiguredMoqData]
        public void Given_ValidCreateEntityAttachmentCommand_When_Validating_Then_IsValid(
            CreateEntityAttachmentCommandValidator validator, CreateEntityAttachmentCommand command)
        {
            // Arrange

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoConfiguredMoqData]
        public void Given_InvalidCreateAttachment_When_DocumentTypeIdIsSetToEmptyGuid_And_Validating_Then_IsNotValid(
            CreateEntityAttachmentCommandValidator validator, CreateEntityAttachmentCommand command)
        {
            // Arrange
            command.EntityId = default(Guid);

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.EntityId), nameof(Messages.notequal_error));
        }

        [Theory]
        [AutoConfiguredMoqData]
        public void Given_InvalidCreateAttachment_When_AttachmentIsNotSet_And_Validating_Then_IsNotValid(
            CreateEntityAttachmentCommandValidator validator, CreateEntityAttachmentCommand command)
        {
            // Arrange
            command.Attachment = null;

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.Attachment), nameof(Messages.notnull_error));
        }

        [Theory]
        [AutoConfiguredMoqData]
        public void Given_CreateAttachment_When_Validating_Then_AttachmentShouldHaveSetValidator(
            CreateEntityAttachmentCommandValidator validator)
        {
            // Assert
            validator.ShouldHaveChildValidator(x => x.Attachment, typeof(CreateAttachmentValidator));
        }
    }
}