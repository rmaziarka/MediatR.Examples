namespace KnightFrank.Antares.Domain.UnitTests.AddressForm.Queries
{
    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.AddressForm.Queries;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("GetCountriesForAddressFormQueryValidator")]
    [Trait("FeatureTitle", "AddressForm")]
    public class GetCountriesForAddressFormQueryValidatorTests
    {
        [Theory]
        [AutoData]
        public void Given_GetCountriesForAddressFormQuery_When_Validating_Then_NoValidationErrors(
           GetCountriesForAddressFormsQueryValidator validator,
           GetCountriesForAddressFormsQuery query)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineAutoData("")]
        [InlineAutoData((string)null)]
        public void Given_IncorrectGetCountriesForAddressFormQueryWithEmptyEntityType_When_Validating_Then_ValidationError(
            string value,
            GetCountriesForAddressFormsQueryValidator validator,
            GetCountriesForAddressFormsQuery query)
        {
            // Arrange
            query.EntityTypeItemCode = value;

            TestIncorrectCommand(validator, query, nameof(query.EntityTypeItemCode));
        }

        private static void TestIncorrectCommand(
            GetCountriesForAddressFormsQueryValidator validator,
            GetCountriesForAddressFormsQuery query,
            string testedPropertyName)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(failure => failure.PropertyName == testedPropertyName);
        }

    }
}