namespace KnightFrank.Antares.Domain.UnitTests.AddressForm.Queries
{
    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.AddressForm.Queries;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("AddressFormQueryValidator")]
    [Trait("FeatureTitle", "AddressForm")]
    public class AddressFormQueryValidatorTests
    {
        [Theory]
        [AutoData]
        public void Given_CorrectAddressFormQuery_When_Validating_Then_NoValidationErrors(
            AddressFormQueryValidator validator,
            AddressFormQuery query)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineAutoData("")]
        [InlineAutoData((string)null)]
        public void Given_IncorrectAddressFormQueryWithEmptyEntityType_When_Validating_Then_ValidationError(
            string value,
            AddressFormQueryValidator validator,
            AddressFormQuery query)
        {
            // Arrange
            query.EntityType = value;

            TestIncorrectCommand(validator, query, nameof(query.EntityType));
        }

        [Theory]
        [InlineAutoData("")]
        [InlineAutoData((string)null)]
        public void Given_IncorrectAddressFormQueryWithEmptyCountryCode_When_Validating_Then_ValidationError(
            string value,
            AddressFormQueryValidator validator,
            AddressFormQuery query)
        {
            // Arrange
            query.CountryCode = value;

            TestIncorrectCommand(validator, query, nameof(query.CountryCode));
        }

        private static void TestIncorrectCommand(
            AddressFormQueryValidator validator,
            AddressFormQuery query,
            string testedPropertyName)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsInvalid(testedPropertyName, nameof(Messages.notempty_error));
        }
    }
}
