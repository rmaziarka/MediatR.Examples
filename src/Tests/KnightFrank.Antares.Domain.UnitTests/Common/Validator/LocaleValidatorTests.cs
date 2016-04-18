namespace KnightFrank.Antares.Domain.UnitTests.Common.Validator
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("LocaleValidator")]
    [Trait("FeatureTitle", "Locale")]
    public class LocaleValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_LocaleDoesNotExistWithThisIsoCode_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode(
            [Frozen] Mock<IGenericRepository<Locale>> localeRepository,
            LocaleValidator validator,
            string isoCode)
        {
            // Arrange
            localeRepository.Setup(r => r.Any(It.IsAny<Expression<Func<Locale, bool>>>())).Returns(false);

            // Act
            ValidationResult validationResult = validator.Validate(isoCode);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(p => p.ErrorCode == "isocodeinvalid_error");
            localeRepository.Verify(r => r.Any(It.IsAny<Expression<Func<Locale, bool>>>()), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public void Given_LocaleExistWithIsoCode_When_Validating_Then_IsValid(
            [Frozen] Mock<IGenericRepository<Locale>> localeRepository,
            LocaleValidator validator,
            string isoCode)
        {
            // Arrange
            localeRepository.Setup(r => r.Any(It.IsAny<Expression<Func<Locale, bool>>>())).Returns(true);
            
            // Act
            ValidationResult validationResult = validator.Validate(isoCode);

            // Assert
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
            localeRepository.Verify(r => r.Any(It.IsAny<Expression<Func<Locale, bool>>>()), Times.Once());
        }
    }
}
