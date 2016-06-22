namespace KnightFrank.Antares.Domain.UnitTests.Company.Commands
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Company.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Company")]
    [Collection("UpdateCompanyValidator")]
    public class UpdateCompanyValidatorTests : IClassFixture<BaseTestClassFixture>
    {
		[Theory]
		[AutoMoqData]
		public void Given_CorrectUpdateCompanyCommand_When_Validating_Then_NoValidationErrors(
			[Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
			UpdateCompanyCommandValidator validator,
			UpdateCompanyCommand cmd)
		{
			// Act
			ValidationResult result = validator.Validate(cmd);

			// Assert
			result.IsValid.Should().BeTrue();
			enumTypeItemValidator.Verify(x => x.ItemExists(EnumType.ClientCareStatus, (Guid) cmd.ClientCareStatusId),
				Times.Once);
		}

		[Theory]
        [AutoMoqData]
        public void Given_EmptyName_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            UpdateCompanyCommandValidator validator,
            UpdateCompanyCommand cmd)
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
        public void Given_NameIsTooLong_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
        UpdateCompanyCommandValidator validator,
        UpdateCompanyCommand cmd)
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
            UpdateCompanyCommandValidator validator,
            UpdateCompanyCommand cmd)
        {
           // Arrange
            cmd.WebsiteUrl = new string('a', 2501);

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
            UpdateCompanyCommandValidator validator,
            UpdateCompanyCommand cmd)
        {
            // Arrange
            cmd.ClientCarePageUrl = new string('a', 2501);

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
            UpdateCompanyCommandValidator validator,
            UpdateCompanyCommand cmd)
        {
            // Arrange
            cmd.Contacts = new List<Contact>();

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.Contacts));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.notempty_error));
        }
    }
}
