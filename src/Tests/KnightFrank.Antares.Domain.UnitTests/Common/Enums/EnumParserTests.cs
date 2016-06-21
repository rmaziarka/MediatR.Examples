namespace KnightFrank.Antares.Domain.UnitTests.Common.Enums
{
    using System;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using PropertyType = KnightFrank.Antares.Dal.Model.Property.PropertyType;

    [Collection("EnumParser")]
    [Trait("FeatureTitle", "ConfigurableAttributes")]
    public class EnumParserTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_IncorrectEnumEntityIdValue_When_ParseIsCalled_Then_ExceptionIsThrown(
            [Frozen] Mock<INinjectInstanceResolver> ninjectInstanceResolver,
            [Frozen] Mock<IGenericRepository<PropertyType>> propertyTypeRepository,
            EnumParser enumParser,
            Guid enumEntityId,
            Domain.Common.Enums.PropertyType propertyTypeEnum)
        {
            // Arrange
            ninjectInstanceResolver.Setup(x => x.GetEntityGenericRepository<PropertyType>()).Returns(propertyTypeRepository.Object);
            propertyTypeRepository.Setup(x => x.GetById(enumEntityId)).Returns(default(PropertyType));

            // Act & Assert
            Assert.Throws<BusinessValidationException>(() => enumParser.Parse<PropertyType, Domain.Common.Enums.PropertyType>(enumEntityId));

            // Assert
            propertyTypeRepository.Verify(x => x.GetById(enumEntityId), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CorrectEnumEntityIdValue_When_ParseIsCalled_Then_ProperEnumValueIsReturned(
            [Frozen] Mock<INinjectInstanceResolver> ninjectInstanceResolver,
            [Frozen] Mock<IGenericRepository<PropertyType>> propertyTypeRepository,
            EnumParser enumParser,
            Guid enumEntityId,
            Domain.Common.Enums.PropertyType propertyTypeEnum,
            PropertyType propertyTypeEntity)
        {
            // Arrange
            string propertyTypeEnumCode = propertyTypeEnum.ToString();
            propertyTypeEntity.EnumCode = propertyTypeEnumCode;

            ninjectInstanceResolver.Setup(x => x.GetEntityGenericRepository<PropertyType>()).Returns(propertyTypeRepository.Object);
            propertyTypeRepository.Setup(x => x.GetById(enumEntityId)).Returns(propertyTypeEntity);

            // Act
            Domain.Common.Enums.PropertyType propertyTypeEnumResult = enumParser.Parse<PropertyType, Domain.Common.Enums.PropertyType>(enumEntityId);

            // Assert
            propertyTypeEnumResult.Should().Be(propertyTypeEnum);
            propertyTypeRepository.Verify(x => x.GetById(enumEntityId), Times.Once);
        }
    }
}
