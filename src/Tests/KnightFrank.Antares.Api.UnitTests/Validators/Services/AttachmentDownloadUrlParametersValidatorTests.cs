namespace KnightFrank.Antares.Api.UnitTests.Validators.Services
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.UnitTests.FixtureExtension;
    using KnightFrank.Antares.Api.Validators.Services;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class AttachmentDownloasUrlParametersValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidAttachmentDownloadUrlParameters_When_Validating_Then_IsValid(
            AttachmentDownloadUrlParameters attachmentUrlParameters,
            AttachmentDownloadUrlParametersValidator validator)
        {
            // Act
            ValidationResult result = validator.Validate(attachmentUrlParameters);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvalidAttachmentDownloadUrlParameters_When_DocumentTypeIdIsNull_Then_IsValid(
            AttachmentDownloadUrlParameters attachmentUrlParameters,
            AttachmentDownloadUrlParametersValidator validator)
        {
            // Arrange
            attachmentUrlParameters.DocumentTypeId = Guid.Empty;

            // Act
            ValidationResult result = validator.Validate(attachmentUrlParameters);

            // Assert
            result.IsInvalid(nameof(attachmentUrlParameters.DocumentTypeId), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvalidAttachmentDownloadUrlParameters_When_ExternalDocumentIdIsNull_Then_IsValid(
            AttachmentDownloadUrlParameters attachmentUrlParameters,
            AttachmentDownloadUrlParametersValidator validator)
        {
            // Arrange
            attachmentUrlParameters.ExternalDocumentId = Guid.Empty;

            // Act
            ValidationResult result = validator.Validate(attachmentUrlParameters);

            // Assert
            result.IsInvalid(nameof(attachmentUrlParameters.ExternalDocumentId), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvalidAttachmentDownloadUrlParameters_When_EntityReferenceIdIsNull_Then_IsValid(
            AttachmentDownloadUrlParameters attachmentUrlParameters,
            AttachmentDownloadUrlParametersValidator validator)
        {
            // Arrange
            attachmentUrlParameters.EntityReferenceId = Guid.Empty;

            // Act
            ValidationResult result = validator.Validate(attachmentUrlParameters);

            // Assert
            result.IsInvalid(nameof(attachmentUrlParameters.EntityReferenceId), nameof(Messages.notempty_error));
        }


        [Theory]
        [InlineAutoData("")]
        [InlineAutoData((string)null)]
        public void Given_InvalidAttachmentDownloadUrlParameters_When_EntityReferenceIdIsNotSet_Then_IsValid(
            string filename,
            AttachmentDownloadUrlParameters attachmentUrlParameters,
            AttachmentDownloadUrlParametersValidator validator)
        {
            // Arrange
            attachmentUrlParameters.Filename = filename;

            // Act
            ValidationResult result = validator.Validate(attachmentUrlParameters);

            // Assert
            result.IsInvalid(nameof(attachmentUrlParameters.Filename), nameof(Messages.notempty_error));
        }

        [Theory]
        [InlineAutoData("")]
        [InlineAutoData((string)null)]
        public void Given_InvalidAttachmentDownloadUrlParameters_When_LocaleIsoCodeIsNotSet_Then_IsValid(
            string localeIsoCode,
            AttachmentDownloadUrlParameters attachmentUrlParameters,
            AttachmentDownloadUrlParametersValidator validator)
        {
            // Arrange
            attachmentUrlParameters.LocaleIsoCode = localeIsoCode;

            // Act
            ValidationResult result = validator.Validate(attachmentUrlParameters);

            // Assert
            result.IsInvalid(nameof(attachmentUrlParameters.LocaleIsoCode), nameof(Messages.notempty_error));
        }
    }
}