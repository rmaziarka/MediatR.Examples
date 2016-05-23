namespace KnightFrank.Antares.Domain.UnitTests.AreaBreakdown.CommandHandlers
{
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "AreaBreakdown")]
    [Collection("PropertyAreaBreakdownValidator")]
    public class PropertyAreaBreakdownValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_AreaBreakdownIsAssignedToProperty_When_Validate_Then_ShouldNotThrowException(
            Property property,
            PropertyAreaBreakdownValidator validator,
            IFixture fixture)
        {
            // Arrange
            PropertyAreaBreakdown propertyAreaBreakdown =
                fixture.Build<PropertyAreaBreakdown>().With(a => a.PropertyId, property.Id).Create();

            // Act
            validator.IsAssignToProperty(propertyAreaBreakdown, property);
        }

        [Theory]
        [AutoMoqData]
        public void Given_AreaBreakdownIsAssignedToOtherProperty_When_Validate_Then_ShouldThrowException(
            PropertyAreaBreakdown propertyAreaBreakdown,
            Property property,
            PropertyAreaBreakdownValidator validator)
        {
            // Act + Assert
            Assert.Throws<BusinessValidationException>(() => validator.IsAssignToProperty(propertyAreaBreakdown, property));
        }
    }
}
