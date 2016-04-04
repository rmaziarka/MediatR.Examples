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

    [Collection("PropertyTypeValidator")]
    [Trait("FeatureTitle", "Property")]
    public class PropertyTypeValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        public PropertyTypeValidatorTests()
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
            [Frozen] Mock<IGenericRepository<PropertyType>> propertyTypeRepository,
            PropertyTypeValidator validator,
            Fixture fixture)
        {
            var id = Guid.NewGuid();

            Expression<Func<IGenericRepository<PropertyType>, PropertyType>> expression = x => x.GetById(id);
            propertyTypeRepository.Setup(expression)
                .Returns(fixture.Create<PropertyType>());

            ValidationResult validationResult = validator.Validate(id);
            
            propertyTypeRepository.Verify(expression, Times.Once);
            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineAutoMoqData]
        public void Given_PropertyTypeNotExist_When_Validating_Then_ValidationErrors(
           [Frozen] Mock<IGenericRepository<PropertyType>> propertyTypeRepository,
           PropertyTypeValidator validator)
        {
            var id = Guid.NewGuid();

            propertyTypeRepository.Setup(x => x.GetById(id))
                .Returns((PropertyType)null);

            ValidationResult validationResult = validator.Validate(id);

            Assert.False(validationResult.IsValid);
        }
    }
}
