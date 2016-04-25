namespace KnightFrank.Antares.Domain.UnitTests.Common.Validator
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CountryValidator")]
    [Trait("FeatureTitle", "Country")]
    public class CountryValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CountryIsCorrect_When_Validating_Then_NoValidationErrors(
            Guid countryId,
            [Frozen] Mock<IGenericRepository<Country>> countryRepository,
            CountryValidator validator)
        {
            // Arrange
            countryRepository.Setup(r => r.Any(It.IsAny<Expression<Func<Country, bool>>>())).Returns(true);

            // Act
            ValidationResult validationResult = validator.Validate(countryId);

            // Asserts
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
            countryRepository.Verify(r => r.Any(It.IsAny<Expression<Func<Country, bool>>>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CountryNotExist_When_Validating_Then_ValidationErrors(
            Guid countryId,
            [Frozen] Mock<IGenericRepository<Country>> countryRepository,
            CountryValidator validator)
        {
            // Arrange
            countryRepository.Setup(r => r.Any(It.IsAny<Expression<Func<Country, bool>>>())).Returns(false);

            // Act
            ValidationResult validationResult = validator.Validate(countryId);

            // Asserts
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(1);
            validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(countryId));
            validationResult.Errors.Should().ContainSingle(p => p.ErrorCode == "countryid_notexist");
            countryRepository.Verify(r => r.Any(It.IsAny<Expression<Func<Country, bool>>>()), Times.Once);
        }
    }
}
