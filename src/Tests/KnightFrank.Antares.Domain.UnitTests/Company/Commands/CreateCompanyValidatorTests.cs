namespace KnightFrank.Antares.Domain.UnitTests.Company.Commands
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Company.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Company")]
    [Collection("CreateCompanyValidator")]
    public class CreateCompanyValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCreateCompanyCommand_When_Validating_Then_IsValid(
            CreateCompanyCommandValidator validator,
            CreateCompanyCommand cmd)
        {
            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyName_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateCompanyCommandValidator validator,
            CreateCompanyCommand cmd)
        {
            // Arrange
            cmd.Name = string.Empty;

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.Name));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_CorrectCreateCompanyCommand_When_Validating_Then_NoValidationErrors(
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
            CreateCompanyCommandValidator validator,
            CreateCompanyCommand cmd)
        {
            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeTrue();
            enumTypeItemValidator.Verify(x => x.ItemExists(EnumType.ClientCareStatus,(Guid) cmd.ClientCareStatusId),
                Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NameIsTooLong_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
        CreateCompanyCommandValidator validator,
        CreateCompanyCommand cmd)
        {
          
            // Arrange
            cmd.Name = new string('a', 129);

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.Name));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.length_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_WebsiteUrlIsTooLong_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateCompanyCommandValidator validator,
            CreateCompanyCommand cmd)
        {
           // Arrange
            cmd.WebsiteUrl = new string('a', 1001);

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.WebsiteUrl));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.length_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ClientCarePageUrlIsTooLong_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateCompanyCommandValidator validator,
            CreateCompanyCommand cmd)
        {
            // Arrange
            cmd.ClientCarePageUrl = new string('a', 1001);

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.ClientCarePageUrl));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.length_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ContactsIdsListIsEmpty_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateCompanyCommandValidator validator,
            CreateCompanyCommand cmd)
        {
            // Arrange
            cmd.ContactIds = new List<Guid>();

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.ContactIds));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.notempty_error));
        }
    }
}
