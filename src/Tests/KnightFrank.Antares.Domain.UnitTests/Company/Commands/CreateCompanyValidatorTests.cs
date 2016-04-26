namespace KnightFrank.Antares.Domain.UnitTests.Company.Commands
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Company.Commands;

    using Xunit;

    [Trait("FeatureTitle", "Company")]
    [Collection("CreateCompanyValidator")]
    public class CreateCompanyValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCreateCompanyCommand_When_Validating_Then_IsValid(
            CreateCompanyValidator validator,
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
            CreateCompanyValidator validator,
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
        public void Given_NameIsNotProvided_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateCompanyValidator validator,
            CreateCompanyCommand cmd)
        {
            // Arrange
            cmd.Name = null;

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.Name));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.notnull_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_NameIsTooLong_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateCompanyValidator validator,
            CreateCompanyCommand cmd)
        {
            // Arrange
            cmd.Name =
                @"gslkfdhglkfdshglkjshfdgklhbvdgfdgsfdgsdfgsfdxcbsjgsghlsfdhglsruiuhlifdshgslurlhgrlsuhglsruhglsdrughlsudrhglsudrhglsghruskuhggsfdfgsfdg";

            // Act
            ValidationResult result = validator.Validate(cmd);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(cmd.Name));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.length_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ContactsIdsListIsEmpty_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            CreateCompanyValidator validator,
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
