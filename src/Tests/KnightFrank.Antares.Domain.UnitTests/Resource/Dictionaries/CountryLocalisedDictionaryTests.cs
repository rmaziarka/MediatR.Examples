namespace KnightFrank.Antares.Domain.UnitTests.Resource.Dictionaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Resource.Dictionaries;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CountryLocalisedDictionary")]
    [Trait("FeatureTitle", "Resources")]
    public class CountryLocalisedDictionaryTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ExistCountryLocalised_When_Handling_Then_ShouldReturnDictionary(
            [Frozen] Mock<IReadGenericRepository<CountryLocalised>> countryLocalisedRepository,
            CountryLocalisedDictionary countryLocalisedDictionary,
            string isoCode,
            IFixture fixture)
        {
            // Arrange
            CountryLocalised countryLocalised = this.CreateCountryLocalised(fixture, isoCode);
            CountryLocalised countryLocalisedWithOtherLocale = this.CreateCountryLocalised(fixture, isoCode + "Other");
            countryLocalisedRepository.Setup(r => r.Get())
                                      .Returns(new[] { countryLocalised, countryLocalisedWithOtherLocale }.AsQueryable());

            // Act
            Dictionary<Guid, string> dictionary = countryLocalisedDictionary.GetDictionary(isoCode);

            // Assert
            dictionary.Should().NotBeNull();
            dictionary.Should().HaveCount(1);
            dictionary.Should().ContainKey(countryLocalised.CountryId);
            dictionary.Should().ContainValue(countryLocalised.Value);
            countryLocalisedRepository.Verify(r => r.Get(), Times.Once());
        }

        private CountryLocalised CreateCountryLocalised(IFixture fixture, string isoCode)
        {
            Locale lcoale = fixture.Build<Locale>().With(l => l.IsoCode, isoCode).Create();
            return fixture.Build<CountryLocalised>().With(el => el.Locale, lcoale).Create();
        }
    }
}
