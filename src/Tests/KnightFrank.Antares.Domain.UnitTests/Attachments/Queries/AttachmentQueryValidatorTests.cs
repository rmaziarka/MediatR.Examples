namespace KnightFrank.Antares.Domain.UnitTests.Attachments.Queries
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Attachment.Queries;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("AttachmentQueryValidator")]
    [Trait("FeatureTitle", "Attachment")]
    public class AttachmentQueryValidatorTests
    {
        [Theory]
        [AutoData]
        public void Given_CorrectAttachmentQuery_When_Validating_Then_IsValid(
           AttachmentQueryValidator validator,
           AttachmentQuery query)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectAttachmentQuery_When_Validating_Then_IsInvalid(
           AttachmentQueryValidator validator,
           AttachmentQuery query)
        {
            // Arrange
            query.Id = default(Guid);

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsInvalid(nameof(query.Id), nameof(Messages.notempty_error));
        }
    }

}