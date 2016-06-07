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

    [Trait("FeatureTitle", "Attachment")]
    [Collection("CreateAttachment")]
    public class CreateAttachmentValidatorTests
    {
        private readonly Fixture fixture;
        private readonly CreateAttachmentValidator validator;

        public CreateAttachmentValidatorTests()
        {
            this.fixture = new Fixture().Customize();
            this.validator = this.fixture.Create<CreateAttachmentValidator>();
        }

        [Fact]
        public void Given_ValidCreateAttachment_When_Validating_Then_IsValid()
        {
            // Arrange 
            CreateAttachment cmd = this.fixture.Create<CreateAttachment>();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Given_InvalidCreateAttachment_When_DocumentTypeIdIsSetToEmptyGuid_And_Validating_Then_IsNotValid()
        {
            // Arrange 
            CreateAttachment cmd =
                this.fixture.Build<CreateAttachment>().With(c => c.DocumentTypeId, default(Guid)).Create();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsInvalid(nameof(cmd.DocumentTypeId), nameof(Messages.notequal_error));
        }

        [Fact]
        public void Given_InvalidCreateAttachment_When_ExternalDocumentIdIsSetToEmptyGuid_And_Validating_Then_IsNotValid()
        {
            // Arrange 
            CreateAttachment cmd =
                this.fixture.Build<CreateAttachment>().With(c => c.ExternalDocumentId, default(Guid)).Create();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsInvalid(nameof(cmd.ExternalDocumentId), nameof(Messages.notequal_error));
        }

        [Theory]
        [InlineData(-1)]
        public void Given_InvalidCreateAttachment_When_SizeIsNegative_And_Validating_Then_IsNotValid(long size)
        {
            // Arrange 
            CreateAttachment cmd =
                this.fixture.Build<CreateAttachment>().With(c => c.Size, size).Create();

            // Act
            ValidationResult validationResult = this.validator.Validate(cmd);

            // Assert
            validationResult.IsInvalid(nameof(cmd.Size), nameof(Messages.greaterthanorequal_error));
        }
    }
}