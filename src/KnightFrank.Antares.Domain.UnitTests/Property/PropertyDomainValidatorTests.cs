namespace KnightFrank.Antares.Domain.UnitTests.Property
{
    using System;
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
            Property property,
            Fixture fixture)
        {
            propertyTypeDefinitionRepository.Setup(x => x.Any(It.IsAny<Expression<Func<PropertyTypeDefinition, bool>>>()))
                                            .Returns(true);

            ValidationResult validationResult = validator.Validate(property);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineAutoMoqData]
        public void Given_PropertyTypeIsNotValidForGivenCountry_When_Validating_Then_ValidationErrors(
           [Frozen] Mock<IGenericRepository<PropertyTypeDefinition>> propertyTypeDefinitionRepository,
           PropertyDomainValidator validator,
           Property property,
           Fixture fixture)
        {
            propertyTypeDefinitionRepository.Setup(x => x.Any(It.IsAny<Expression<Func<PropertyTypeDefinition, bool>>>()))
                                            .Returns(false);

            ValidationResult validationResult = validator.Validate(property);

            Assert.False(validationResult.IsValid);
        }
    }
}
