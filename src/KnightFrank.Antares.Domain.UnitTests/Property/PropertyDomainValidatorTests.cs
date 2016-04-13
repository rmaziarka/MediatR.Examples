namespace KnightFrank.Antares.Domain.UnitTests.Property
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Property.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("PropertyDomainValidator")]
    [Trait("FeatureTitle", "Property")]
    public class PropertyDomainValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        public PropertyDomainValidatorTests()
        {
            IFixture fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            fixture.Behaviors.Clear();
            fixture.RepeatCount = 1;
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Theory]
        [InlineAutoMoqData]
        public void Given_PropertyTypeIsCorrect_When_Validating_Then_NoValidationErrors(
            [Frozen] Mock<IGenericRepository<PropertyTypeDefinition>> propertyTypeDefinitionRepository,
            PropertyDomainValidator validator,
            Property property)
        {
            // Arrange
            var propertyTypeDefinitions = new List<PropertyTypeDefinition>
            {
                new PropertyTypeDefinition
                {
                    CountryId = property.Address.CountryId,
                    DivisionId = property.DivisionId,
                    PropertyTypeId = property.PropertyTypeId
                }
            };

            propertyTypeDefinitionRepository.Setup(x => x.Any(It.IsAny<Expression<Func<PropertyTypeDefinition, bool>>>()))
                                            .Returns(
                                                new Func<Expression<Func<PropertyTypeDefinition, bool>>, bool>(
                                                    expr => propertyTypeDefinitions.Any(expr.Compile())));

            // Act
            ValidationResult validationResult = validator.Validate(property);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineAutoMoqData]
        public void Given_PropertyTypeIsNotValidForGivenCountry_When_Validating_Then_ValidationErrors(
           [Frozen] Mock<IGenericRepository<PropertyTypeDefinition>> propertyTypeDefinitionRepository,
           PropertyDomainValidator validator,
           Property property)
        {
            // Arrange
            var propertyTypeDefinitions = new List<PropertyTypeDefinition>
            {
                new PropertyTypeDefinition
                {
                    CountryId = Guid.Empty,
                    DivisionId = property.DivisionId,
                    PropertyTypeId = property.PropertyTypeId
                },
                new PropertyTypeDefinition
                {
                    CountryId = property.Address.CountryId,
                    DivisionId = Guid.Empty,
                    PropertyTypeId = property.PropertyTypeId
                },
                new PropertyTypeDefinition
                {
                    CountryId = property.Address.CountryId,
                    DivisionId = property.DivisionId,
                    PropertyTypeId = Guid.Empty,
                }
            };

            // Act 
            propertyTypeDefinitionRepository.Setup(x => x.Any(It.IsAny<Expression<Func<PropertyTypeDefinition, bool>>>()))
                                            .Returns(
                                                new Func<Expression<Func<PropertyTypeDefinition, bool>>, bool>(
                                                    expr => propertyTypeDefinitions.Any(expr.Compile())));


            ValidationResult validationResult = validator.Validate(property);

            // Assert
            Assert.False(validationResult.IsValid);
        }
    }
}
