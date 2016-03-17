namespace KnightFrank.Antares.Domain.UnitTests.AddressForm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AddressForm;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;

    [Collection("AddressFormQueryHandler")]
    [Trait("FeatureTitle", "AddressForm")]
    public class AddressFormQueryHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Mock<IReadGenericRepository<Country>> countryRepository;
        private readonly Mock<IReadGenericRepository<EnumTypeItem>> enumTypeItemRepository;
        private readonly Mock<IReadGenericRepository<AddressForm>> addressFormRepository;
        private readonly ICollection<Country> mockedCountryData;
        private readonly ICollection<EnumTypeItem> mockedEnumTypeItemData;
        private readonly List<AddressForm> mockedAddressFormData;
        private readonly AddressFormQueryHandler handler;

        public AddressFormQueryHandlerTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.Clear();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.countryRepository = fixture.Freeze<Mock<IReadGenericRepository<Country>>>();
            this.enumTypeItemRepository = fixture.Freeze<Mock<IReadGenericRepository<EnumTypeItem>>>();
            this.addressFormRepository = fixture.Freeze<Mock<IReadGenericRepository<AddressForm>>>();
            this.mockedCountryData = fixture.CreateMany<Country>().ToList();
            this.mockedEnumTypeItemData = fixture.CreateMany<EnumTypeItem>().ToList();
            this.mockedAddressFormData = fixture.CreateMany<AddressForm>().ToList();
            this.handler = fixture.Create<AddressFormQueryHandler>();
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void Given_InvalidCountryInQuery_When_HandlingQuery_Then_ShouldThrowException(
            string enumTypeCode,
            string entityTypeCode,
            Country country,
            IFixture fixture)
        {
            // Arrange
            AddressFormQuery query = fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);
            EnumTypeItem enumTypeItem = fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            // Act / Assert
            Assert.Throws<DomainValidationException>(() => this.handler.Handle(query)).Message.Should().Be("message.CountryCode");
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void Given_InvalidEnumTypeItemInQuery_When_HandlingQuery_Then_ShouldThrowException(
            string enumTypeCode,
            string entityTypeCode,
            Country country,
            IFixture fixture)
        {
            // Arrange
            AddressFormQuery query = fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);

            this.mockedCountryData.Add(country);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            // Act / Assert
            Assert.Throws<DomainValidationException>(() => this.handler.Handle(query)).Message.Should().Be("message.EntityType");
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void Given_EnumTypeItemExistsButForInvalidEnumTypeInQuery_When_HandlingQuery_Then_ShouldThrowException(
            string enumTypeCode,
            string entityTypeCode,
            string otherEnumTypeCode,
            Country country,
            IFixture fixture)
        {
            // Arrange
            AddressFormQuery query = fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);
            EnumTypeItem enumTypeItem = fixture.BuildEnumTypeItem(otherEnumTypeCode, entityTypeCode);

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            // Act / Assert
            Assert.Throws<DomainValidationException>(() => this.handler.Handle(query)).Message.Should().Be("message.EntityType");
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void
            Given_ExistingAddressForCountryAndEntityTypeInQuery_When_HandlingQuery_Then_ShouldReturnAddressForCountryAndEntityType(
            string enumTypeCode,
            string entityTypeCode,
            Country country,
            Country otherCountry,
            EnumTypeItem otherEnumTypeItem,
            IFixture fixture)
        {
            // Arrange
            AddressFormQuery query = fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);
            EnumTypeItem enumTypeItem = fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            AddressForm addressFormForOtherCountry = fixture.BuildAddressForm(enumTypeItem, otherCountry);
            AddressForm addressFormForOtherEntityType = fixture.BuildAddressForm(otherEnumTypeItem, country);
            AddressForm addressFormForEntityType = fixture.BuildAddressForm(enumTypeItem, country);

            this.mockedAddressFormData.AddRange(new[]
            { addressFormForEntityType, addressFormForOtherCountry, addressFormForOtherEntityType });
            this.addressFormRepository.Setup(x => x.Get()).Returns(this.mockedAddressFormData.AsQueryable());

            // Act
            AddressFormQueryResult queryResult = this.handler.Handle(query);

            // Assert
            queryResult.Should().NotBeNull();
            queryResult.Id.Should().Be(addressFormForEntityType.Id);
            queryResult.CountryId.Should().Be(country.Id);
            queryResult.AddressFieldDefinitions.Should().NotBeEmpty();
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void Given_NotExistingAddressForEntityTypeInQuery_HandlingQuery_Then_ShouldReturnDefaultAddressForCountry(
            string enumTypeCode,
            string entityTypeCode,
            Country country,
            Country otherCountry,
            EnumTypeItem otherEnumTypeItem,
            IFixture fixture)
        {
            // Arrange
            AddressFormQuery query = fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);
            EnumTypeItem enumTypeItem = fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            AddressForm addressFormForOtherCountry = fixture.BuildAddressForm(enumTypeItem, otherCountry);
            AddressForm addressFormForOtherEntityType = fixture.BuildAddressForm(otherEnumTypeItem, country);
            AddressForm defaultAddressForm = fixture.BuildAddressForm(null, country);

            this.mockedAddressFormData.AddRange(new[]
            { defaultAddressForm, addressFormForOtherCountry, addressFormForOtherEntityType });
            this.addressFormRepository.Setup(x => x.Get()).Returns(this.mockedAddressFormData.AsQueryable());

            // Act
            AddressFormQueryResult queryResult = this.handler.Handle(query);

            // Assert
            queryResult.Should().NotBeNull();
            queryResult.Id.Should().Be(defaultAddressForm.Id);
            queryResult.CountryId.Should().Be(country.Id);
            queryResult.AddressFieldDefinitions.Should().NotBeEmpty();
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void Given_NotExistingAddressForCountryAndEntityTypeInQuery_HandlingQuery_Then_ShouldReturnNull(
            string enumTypeCode,
            string entityTypeCode,
            Country country,
            Country otherCountry,
            EnumTypeItem otherEnumTypeItem,
            IFixture fixture)
        {
            // Arrange
            AddressFormQuery query = fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);
            EnumTypeItem enumTypeItem = fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            AddressForm addressFormForOtherCountry = fixture.BuildAddressForm(enumTypeItem, otherCountry);
            AddressForm addressFormForOtherEntityType = fixture.BuildAddressForm(otherEnumTypeItem, country);

            this.mockedAddressFormData.AddRange(new[] { addressFormForOtherCountry, addressFormForOtherEntityType });
            this.addressFormRepository.Setup(x => x.Get()).Returns(this.mockedAddressFormData.AsQueryable());

            // Act
            AddressFormQueryResult queryResult = this.handler.Handle(query);

            // Assert
            queryResult.Should().BeNull();
        }
    }
}
