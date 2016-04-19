namespace KnightFrank.Antares.Domain.UnitTests.Resource.Dictionaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Resource.Dictionaries;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("PropertyTypeLocalisedDictionary")]
    [Trait("FeatureTitle", "Resources")]
    public class PropertyTypeLocalisedDictionaryTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ExistPropertyTypeLocalised_When_Handling_Then_ShouldReturnDictionary(
            [Frozen] Mock<IReadGenericRepository<PropertyTypeLocalised>> propertyTypeLocalisedRepository,
            PropertyTypeLocalisedDictionary propertyTypeLocalisedDictionary,
            string isoCode,
            IFixture fixture)
        {
            // Arrange
            PropertyTypeLocalised propertyTypeLocalised = this.CreatePropertyTypeLocalised(fixture, isoCode);
            PropertyTypeLocalised propertyTypeLocalisedWithOtherLocale = this.CreatePropertyTypeLocalised(
                fixture,
                isoCode + "Other");
            propertyTypeLocalisedRepository.Setup(r => r.Get())
                                           .Returns(
                                               new[] { propertyTypeLocalised, propertyTypeLocalisedWithOtherLocale }.AsQueryable());

            // Act
            Dictionary<Guid, string> dictionary = propertyTypeLocalisedDictionary.GetDictionary(isoCode);

            // Assert
            dictionary.Should().NotBeNull();
            dictionary.Should().HaveCount(1);
            dictionary.Should().ContainKey(propertyTypeLocalised.PropertyTypeId);
            dictionary.Should().ContainValue(propertyTypeLocalised.Value);
            propertyTypeLocalisedRepository.Verify(r => r.Get(), Times.Once());
        }

        private PropertyTypeLocalised CreatePropertyTypeLocalised(IFixture fixture, string isoCode)
        {
            Locale lcoale = fixture.Build<Locale>().With(l => l.IsoCode, isoCode).Create();
            return fixture.Build<PropertyTypeLocalised>().With(el => el.Locale, lcoale).Create();
        }
    }
}
