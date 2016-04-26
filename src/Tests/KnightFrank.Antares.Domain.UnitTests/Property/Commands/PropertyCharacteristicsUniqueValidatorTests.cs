namespace KnightFrank.Antares.Domain.UnitTests.Property.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Property.Commands;

    using Ploeh.AutoFixture;

    using Xunit;

    [Collection("PropertyCharacteristicsUniqueValidator")]
    [Trait("FeatureTitle", "Property")]
    public class PropertyCharacteristicsUniqueValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_UniqueCharacteristicIdList_When_Validating_Then_IsValid(
            PropertyCharacteristicsUniqueValidator validator,
            IFixture fixture)
        {
            // Arrange
            IList<CreateOrUpdatePropertyCharacteristic> propertyCharacteristics =
                new[]
                    {
                        this.CreateCreateOrUpdatePropertyCharacteristic(fixture, Guid.NewGuid()),
                        this.CreateCreateOrUpdatePropertyCharacteristic(fixture, Guid.NewGuid())
                    }.ToList();

            // Act
            ValidationResult result = validator.Validate(propertyCharacteristics);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_DuplicatedCharacteristicIdList_When_Validating_Then_IsInvalid(
            PropertyCharacteristicsUniqueValidator validator,
            Guid characteristicId,
            IFixture fixture)
        {
            // Arrange
            IList<CreateOrUpdatePropertyCharacteristic> propertyCharacteristics =
                new[]
                    {
                        this.CreateCreateOrUpdatePropertyCharacteristic(fixture, characteristicId),
                        this.CreateCreateOrUpdatePropertyCharacteristic(fixture, characteristicId)
                    }.ToList();

            // Act
            ValidationResult result = validator.Validate(propertyCharacteristics);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().Contain(e => e.PropertyName == "propertyCharacteristics");
            result.Errors.Should().Contain(e => e.ErrorCode == "propertyCharacteristics_duplicated");
        }

        private CreateOrUpdatePropertyCharacteristic CreateCreateOrUpdatePropertyCharacteristic(
            IFixture fixture,
            Guid characteristicId)
        {
            return fixture.Build<CreateOrUpdatePropertyCharacteristic>().With(p => p.CharacteristicId, characteristicId).Create();
        }
    }
}
