namespace KnightFrank.Antares.Domain.UnitTests.Property.Commands
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("PropertyTypeValidator")]
    [Trait("FeatureTitle", "Property")]
    public class PropertyTypeValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_PropertyTypeIsCorrect_When_Validating_Then_NoValidationErrors(
            [Frozen] Mock<IGenericRepository<PropertyType>> propertyTypeRepository,
            PropertyTypeValidator validator)
        {
            // Arrange
            Guid propertyTypeId = Guid.NewGuid();
            propertyTypeRepository.Setup(r => r.Any(It.IsAny<Expression<Func<PropertyType, bool>>>())).Returns(true);

            // Act
            ValidationResult validationResult = validator.Validate(propertyTypeId);

            // Asserts
            validationResult.IsValid.Should().BeTrue();
            propertyTypeRepository.Verify(r => r.Any(It.IsAny<Expression<Func<PropertyType, bool>>>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_PropertyTypeNotExist_When_Validating_Then_ValidationErrors(
            [Frozen] Mock<IGenericRepository<PropertyType>> propertyTypeRepository,
            PropertyTypeValidator validator)
        {
            // Arrange
            Guid id = Guid.NewGuid();
            propertyTypeRepository.Setup(r => r.Any(It.IsAny<Expression<Func<PropertyType, bool>>>())).Returns(false);
            
            // Act
            ValidationResult validationResult = validator.Validate(id);

            // Asserts
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(1);
            validationResult.Errors.Should().ContainSingle(p => p.ErrorCode == "propertytypeid_error");
            propertyTypeRepository.Verify(r => r.Any(It.IsAny<Expression<Func<PropertyType, bool>>>()), Times.Once);
        }
    }
}
