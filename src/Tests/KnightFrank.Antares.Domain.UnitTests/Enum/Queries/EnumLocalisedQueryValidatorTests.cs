namespace KnightFrank.Antares.Domain.UnitTests.Enum.Queries
{
    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Enum.Queries;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Xunit;

    [Collection("EnumLocalisedQueryValidator")]
    [Trait("FeatureTitle", "Enums")]
    public class EnumLocalisedQueryValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidEnumLocalisedQuery_When_Validating_Then_IsValid(
            EnumLocalisedQueryValidator validator,
            EnumLocalisedQuery query)
        {
            // Arrange
            query.IsoCode = "en";

            // Act
            ValidationResult result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyIsoCode_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            EnumLocalisedQueryValidator validator,
            EnumLocalisedQuery query)
        {
            // Arrange
            query.IsoCode = string.Empty;

            // Act
            ValidationResult result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(query.IsoCode));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IsoCodeIsNotProvided_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            EnumLocalisedQueryValidator validator,
            EnumLocalisedQuery query)
        {
            // Arrange
            query.IsoCode = null;

            // Act
            ValidationResult result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(query.IsoCode));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.notnull_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IsoCodeIsTooLong_When_Validating_Then_IsInvalidAndHasAppropriateErrorMsg(
            EnumLocalisedQueryValidator validator,
            EnumLocalisedQuery query)
        {
            // Arrange
            query.IsoCode = "asd";

            // Act
            ValidationResult result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(query.IsoCode));
            result.Errors.Should().Contain(e => e.ErrorCode == nameof(Messages.length_error));
        }
    }
}
