namespace KnightFrank.Antares.Domain.UnitTests.Attachments.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Domain.Attachment.Commands;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Ploeh.AutoFixture;

    using Xunit;

    [Trait("FeatureTitle", "Property Entity")]
    [Collection("CreateEntityAttachment")]
    public class CreateEntityAttachmentCommandValidatorTests
    {
        private readonly Fixture fixture;
        private readonly CreateEntityAttachmentCommandValidator validator;

        private class TestCommand : CreateEntityAttachmentCommand
        {
        }

        public CreateEntityAttachmentCommandValidatorTests()
        {
            this.fixture = new Fixture().Customize();
            this.validator = this.fixture.Create<CreateEntityAttachmentCommandValidator>();
        }

        [Fact]
        public void Given_ValidCreateEntityAttachmentCommand_When_Validating_Then_IsValid()
        {
            // Arrange
            var cmd = this.fixture.Create<TestCommand>();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Given_InvalidCreateAttachment_When_DocumentTypeIdIsSetToEmptyGuid_And_Validating_Then_IsNotValid()
        {
            // Arrange
            TestCommand cmd =
                this.fixture.Build<TestCommand>().With(c => c.EntityId, default(Guid)).Create();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsInvalid(nameof(cmd.EntityId), nameof(Messages.notequal_error));
        }

        [Fact]
        public void Given_InvalidCreateAttachment_When_AttachmentIsNotSet_And_Validating_Then_IsNotValid()
        {
            // Arrange
            TestCommand cmd =
                this.fixture.Build<TestCommand>().With(c => c.Attachment, null).Create();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsInvalid(nameof(cmd.Attachment), nameof(Messages.notnull_error));
        }

        [Fact]
        public void Given_CreateAttachment_When_Validating_Then_AttachmentShouldHaveSetValidator()
        {
            // Assert
           this.validator.ShouldHaveChildValidator(x => x.Attachment, typeof(CreateAttachmentValidator));
        }
    }
}