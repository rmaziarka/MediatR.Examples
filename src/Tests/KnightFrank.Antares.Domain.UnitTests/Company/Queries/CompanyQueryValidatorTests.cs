namespace KnightFrank.Antares.Domain.UnitTests.Company.Queries
{
    using System;

    using FluentAssertions;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Company.Queries;
    
    public class CompanyQueryValidatorTests
    {
        private const string NotEmptyError = "notempty_error";

        [Theory]
        [AutoData]
        public void Given_CorrectCompanyQuery_When_Validating_Then_IsValid(
            CompanyQueryValidator validator,
            CompanyQuery query)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
     [AutoData]
        public void Given_CompanyIdIsEmptyGuid_When_Validating_Then_IsInvalid(string input,

            CompanyQueryValidator validator,
            CompanyQuery query)
        {
            // Arrange
            query.Id = default(Guid);

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(query.Id));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == NotEmptyError);
        }

    }

}