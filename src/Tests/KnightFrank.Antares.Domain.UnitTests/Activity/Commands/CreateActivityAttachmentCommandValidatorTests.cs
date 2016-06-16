namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Attachment.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Ploeh.AutoFixture;

    using Xunit;

    [Trait("FeatureTitle", "Property Activity")]
    [Collection("CreateActivityAttachment")]
    public class CreateActivityAttachmentCommandValidatorTests
    {
        private readonly Fixture fixture;
        private readonly CreateActivityAttachmentCommandValidator validator;

        public CreateActivityAttachmentCommandValidatorTests()
        {
            this.fixture = new Fixture().Customize();
            this.validator = this.fixture.Create<CreateActivityAttachmentCommandValidator>();
        }

        [Fact]
        public void Given_ValidCreateActivityAttachmentCommand_When_Validating_Then_IsValid()
        {
            // Arrange 
            CreateActivityAttachmentCommand cmd = this.fixture.Create<CreateActivityAttachmentCommand>();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Given_InvalidCreateAttachment_When_DocumentTypeIdIsSetToEmptyGuid_And_Validating_Then_IsNotValid()
        {
            // Arrange 
            CreateActivityAttachmentCommand cmd =
                this.fixture.Build<CreateActivityAttachmentCommand>().With(c => c.ActivityId, default(Guid)).Create();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsInvalid(nameof(cmd.ActivityId), nameof(Messages.notequal_error));
        }

        [Fact]
        public void Given_InvalidCreateAttachment_When_AttachmentIsNotSet_And_Validating_Then_IsNotValid()
        {
            // Arrange 
            CreateActivityAttachmentCommand cmd =
                this.fixture.Build<CreateActivityAttachmentCommand>().With(c => c.Attachment, null).Create();

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