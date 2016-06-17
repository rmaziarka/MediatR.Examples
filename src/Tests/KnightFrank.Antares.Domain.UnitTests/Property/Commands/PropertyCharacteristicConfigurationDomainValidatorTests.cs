namespace KnightFrank.Antares.Domain.UnitTests.Property.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Property.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("PropertyCharacteristicConfigurationDomainValidator")]
    [Trait("FeatureTitle", "Property")]
    public class PropertyCharacteristicConfigurationDomainValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectConfigurationForCharacteristic_When_Validating_Then_IsValid(
            Guid propertyTypeId,
            Guid countryId,
            Mock<IGenericRepository<CharacteristicGroupUsage>> characteristicGroupUsageRepository,
            CreateOrUpdatePropertyCharacteristic createOrUpdatePropertyCharacteristic,
            IFixture fixture)
        {
            // Arrange
            CharacteristicGroupItem characteristicGroupItem =
                fixture.Build<CharacteristicGroupItem>()
                       .With(i => i.CharacteristicId, createOrUpdatePropertyCharacteristic.CharacteristicId)
                       .Create();
            CharacteristicGroupUsage characteristicGroupUsage =
                fixture.Build<CharacteristicGroupUsage>()
                       .With(gu => gu.PropertyTypeId, propertyTypeId)
                       .With(gu => gu.CountryId, countryId)
                       .With(gu => gu.CharacteristicGroupItems, new[] { characteristicGroupItem })
                       .Create();

            characteristicGroupUsageRepository.Setup(
                r =>
                r.GetWithInclude(It.IsAny<Expression<Func<CharacteristicGroupUsage, bool>>>(), cgu => cgu.CharacteristicGroupItems))
                                              .Returns(new[] { characteristicGroupUsage }.AsEnumerable());

            var validator = new PropertyCharacteristicConfigurationDomainValidator(
                characteristicGroupUsageRepository.Object,
                propertyTypeId,
                countryId);

            // Act
            ValidationResult result = validator.Validate(new[] { createOrUpdatePropertyCharacteristic });

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_NoneConfigurationForCharacteristic_When_Validating_Then_IsInvalid(
            [Frozen] Mock<IGenericRepository<CharacteristicGroupUsage>> characteristicGroupUsageRepository,
            PropertyCharacteristicConfigurationDomainValidator validator,
            List<CreateOrUpdatePropertyCharacteristic> createOrUpdatePropertyCharacteristics)
        {
            // Arrange
            characteristicGroupUsageRepository.Setup(
                r =>
                r.GetWithInclude(It.IsAny<Expression<Func<CharacteristicGroupUsage, bool>>>(), cgu => cgu.CharacteristicGroupItems))
                                              .Returns(new CharacteristicGroupUsage[] { }.AsEnumerable());

            // Act
            ValidationResult result = validator.Validate(createOrUpdatePropertyCharacteristics);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().Contain(e => e.PropertyName == "propertyCharacteristics");
            result.Errors.Should().Contain(e => e.ErrorCode == "propertyCharacteristics_notconfigured");
        }
    }
}
