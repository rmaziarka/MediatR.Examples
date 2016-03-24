namespace KnightFrank.Antares.Domain.UnitTests.AddressForm.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AddressForm.QueryHandlers;
    using KnightFrank.Antares.Domain.AddressForm.QueryResults;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;

    [Collection("GetCountriesForAddressFormQueryHandler")]
    [Trait("FeatureTitle", "AddressForm")]
    public class GetCountriesForAddressFormQueryHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Mock<IReadGenericRepository<EnumTypeItem>> enumTypeItemRepository;
        private readonly Mock<IReadGenericRepository<AddressForm>> addressFormRepository;
        private readonly ICollection<Country> mockedCountryData;
        private readonly ICollection<EnumTypeItem> mockedEnumTypeItemData;
        private readonly List<AddressForm> mockedAddressFormData;
        private readonly GetCountriesForAddressFormQueryHandler handler;

        private readonly IFixture fixture;

        public GetCountriesForAddressFormQueryHandlerTests()
        {
            this.fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            this.fixture.Behaviors.Clear();

            this.fixture.RepeatCount = 1;
            this.fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.enumTypeItemRepository = this.fixture.Freeze<Mock<IReadGenericRepository<EnumTypeItem>>>();
            this.addressFormRepository = this.fixture.Freeze<Mock<IReadGenericRepository<AddressForm>>>();
            this.mockedCountryData = this.fixture.CreateMany<Country>().ToList();
            this.mockedEnumTypeItemData = this.fixture.CreateMany<EnumTypeItem>().ToList();
            this.mockedAddressFormData = this.fixture.CreateMany<AddressForm>().ToList();
            this.handler = this.fixture.Create<GetCountriesForAddressFormQueryHandler>();
        }

        [Theory]
        [InlineAutoMoqData]
        public void Given_InvalidEntityTypeInQuery_When_HandlingQuery_Then_ShouldThrowException(string entityTypeCode)
        {
            // Arrange
            var query = this.fixture.BuildGetCountriesForAddressFormQuery(entityTypeCode, null);

            // Act + Assert
            Assert.Throws<DomainValidationException>(() => this.handler.Handle(query)).Message.Should().Be("message.EntityType");
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void
            Given_TwoCountriesForGivenEntityType_When_OneIsDefinedGloballyAndOnePerEntity_Then_BothCountriesShouldBeReturned(
            string enumTypeCode,
            string entityTypeCode,
            string localeIsoCode,
            Country country,
            Country otherCountry)
        {
            var query = this.fixture.BuildGetCountriesForAddressFormQuery(entityTypeCode, localeIsoCode);
            EnumTypeItem enumTypeItem = this.fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            country.CountryLocaliseds.Add(this.fixture.BuildCountryLocalised(localeIsoCode));
            otherCountry.CountryLocaliseds.Add(this.fixture.BuildCountryLocalised(localeIsoCode));

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            AddressForm addressFormForCountry = this.fixture.BuildAddressForm(enumTypeItem, country);
            AddressForm addressFormDefinedGloballyForOtherCountry = this.fixture.BuildAddressForm(null, otherCountry);

            this.mockedAddressFormData.AddRange(new[] { addressFormForCountry, addressFormDefinedGloballyForOtherCountry });
            this.addressFormRepository.Setup(x => x.Get()).Returns(this.mockedAddressFormData.AsQueryable());

            // Act
            List<CountryLocalisedResult> countryLocaliseds = this.handler.Handle(query);

            // Assert
            countryLocaliseds.Should().NotBeNull();
            countryLocaliseds.Should().HaveCount(2);
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void
            Given_CountryForGivenEntityType_When_AdressFormIsDefinedGloballyAndPerEntity_Then_OneCountryShouldBeReturned(
            string enumTypeCode,
            string entityTypeCode,
            string localeIsoCode,
            string localeValue,
            Country country)
        {
            var query = this.fixture.BuildGetCountriesForAddressFormQuery(entityTypeCode, localeIsoCode);
            EnumTypeItem enumTypeItem = this.fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            country.CountryLocaliseds.Add(this.fixture.BuildCountryLocalised(localeIsoCode, localeValue));

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            AddressForm addressFormForCountry = this.fixture.BuildAddressForm(enumTypeItem, country);
            AddressForm addressFormDefinedGloballyForOtherCountry = this.fixture.BuildAddressForm(null, country);

            this.mockedAddressFormData.AddRange(new[] { addressFormForCountry, addressFormDefinedGloballyForOtherCountry });
            this.addressFormRepository.Setup(x => x.Get()).Returns(this.mockedAddressFormData.AsQueryable());

            // Act
            List<CountryLocalisedResult> countryLocaliseds = this.handler.Handle(query);

            // Assert
            AssertSingleLocalisedCountryWithValue(localeValue, countryLocaliseds);
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void
            Given_CountryForGivenEntityType_When_OtherCountryForOtherEntityTypeIsGiven_Then_OneCountryShouldBeReturned(
            string enumTypeCode,
            string entityTypeCode,
            string otherEntityTypeCode,
            string localeIsoCode,
            string localeValue,
            Country country,
            Country otherCountry)
        {
            var query = this.fixture.BuildGetCountriesForAddressFormQuery(entityTypeCode, localeIsoCode);
            EnumType enumType = this.fixture.BuildEnumType(enumTypeCode);

            EnumTypeItem enumTypeItem = this.fixture.BuildEnumTypeItem(enumType, entityTypeCode);
            EnumTypeItem otherEnumTypeItem = this.fixture.BuildEnumTypeItem(enumType, otherEntityTypeCode);

            country.CountryLocaliseds.Add(this.fixture.BuildCountryLocalised(localeIsoCode, localeValue));
            otherCountry.CountryLocaliseds.Add(this.fixture.BuildCountryLocalised(localeIsoCode));

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            AddressForm addressFormForCountry = this.fixture.BuildAddressForm(enumTypeItem, country);
            AddressForm addressFormDefinedGloballyForOtherCountry = this.fixture.BuildAddressForm(otherEnumTypeItem, country);

            this.mockedAddressFormData.AddRange(new[] { addressFormForCountry, addressFormDefinedGloballyForOtherCountry });
            this.addressFormRepository.Setup(x => x.Get()).Returns(this.mockedAddressFormData.AsQueryable());

            // Act
            List<CountryLocalisedResult> countryLocaliseds = this.handler.Handle(query);

            // Assert
            countryLocaliseds.Should().NotBeNull();
            countryLocaliseds.Should().HaveCount(1);
            CountryLocalisedResult countryLocalised = countryLocaliseds.Single();

            countryLocalised.Value.Should().Be(localeValue);
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void
            Given_CountryForGivenEntityType_When_TwoLocalizationAreConfigured_Then_CorrectLocaleShouldBeReturned(
            string enumTypeCode,
            string entityTypeCode,
            string localeIsoCode,
            string otherLocaleIsoCode,
            string localeValue,
            Country country)
        {
            var query = this.fixture.BuildGetCountriesForAddressFormQuery(entityTypeCode, localeIsoCode);
            EnumTypeItem enumTypeItem = this.fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            country.CountryLocaliseds.Add(this.fixture.BuildCountryLocalised(localeIsoCode, localeValue));
            country.CountryLocaliseds.Add(this.fixture.BuildCountryLocalised(otherLocaleIsoCode));

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            AddressForm addressFormForCountry = this.fixture.BuildAddressForm(enumTypeItem, country);

            this.mockedAddressFormData.AddRange(new[] { addressFormForCountry });
            this.addressFormRepository.Setup(x => x.Get()).Returns(this.mockedAddressFormData.AsQueryable());

            // Act
            List<CountryLocalisedResult> countryLocaliseds = this.handler.Handle(query);

            // Assert
            AssertSingleLocalisedCountryWithValue(localeValue, countryLocaliseds);
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void
            Given_CountryForGivenEntityType_When_NoLocalizationIsConfigured_Then_NoCountryShouldBeReturned(
            string enumTypeCode,
            string entityTypeCode,
            string localeIsoCode,
            string otherLocaleIsoCode,
            Country country)
        {
            var query = this.fixture.BuildGetCountriesForAddressFormQuery(entityTypeCode, localeIsoCode);
            EnumTypeItem enumTypeItem = this.fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            AddressForm addressFormForCountry = this.fixture.BuildAddressForm(enumTypeItem, country);

            this.mockedAddressFormData.AddRange(new[] { addressFormForCountry });
            this.addressFormRepository.Setup(x => x.Get()).Returns(this.mockedAddressFormData.AsQueryable());

            // Act
            List<CountryLocalisedResult> countryLocaliseds = this.handler.Handle(query);

            // Assert
            countryLocaliseds.Should().BeEmpty();
        }

        private static void AssertSingleLocalisedCountryWithValue(string localeValue, List<CountryLocalisedResult> countryLocaliseds)
        {
            countryLocaliseds.Should().NotBeNull();
            countryLocaliseds.Should().HaveCount(1);
            CountryLocalisedResult countryLocalised = countryLocaliseds.Single();

            countryLocalised.Value.Should().Be(localeValue);
        }
    }
}