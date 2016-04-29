namespace KnightFrank.Antares.Domain.UnitTests.AddressForm.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AddressForm.Queries;
    using KnightFrank.Antares.Domain.AddressForm.QueryHandlers;
    using KnightFrank.Antares.Domain.AddressForm.QueryResults;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;

    [Collection("AddressFormQueryHandler")]
    [Trait("FeatureTitle", "AddressForm")]
    public class AddressFormQueryHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Mock<IReadGenericRepository<AddressForm>> addressFormRepository;

        private readonly Mock<IReadGenericRepository<Country>> countryRepository;

        private readonly Mock<IReadGenericRepository<EnumTypeItem>> enumTypeItemRepository;

        private readonly AddressFormQueryHandler handler;

        private readonly List<AddressForm> mockedAddressFormData;

        private readonly ICollection<Country> mockedCountryData;

        private readonly ICollection<EnumTypeItem> mockedEnumTypeItemData;

        private readonly IFixture fixture;

        public AddressFormQueryHandlerTests()
        {
            this.fixture = new Fixture().Customize(new AutoMoqCustomization());
            this.fixture.Behaviors.Clear();
            this.fixture.RepeatCount = 1;
            this.fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.countryRepository = this.fixture.Freeze<Mock<IReadGenericRepository<Country>>>();
            this.enumTypeItemRepository = this.fixture.Freeze<Mock<IReadGenericRepository<EnumTypeItem>>>();
            this.addressFormRepository = this.fixture.Freeze<Mock<IReadGenericRepository<AddressForm>>>();
            this.mockedCountryData = this.fixture.CreateMany<Country>().ToList();
            this.mockedEnumTypeItemData = this.fixture.CreateMany<EnumTypeItem>().ToList();
            this.mockedAddressFormData = this.fixture.CreateMany<AddressForm>().ToList();
            this.handler = this.fixture.Create<AddressFormQueryHandler>();
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void Given_InvalidCountryInQuery_When_HandlingQuery_Then_ShouldThrowException(
            string enumTypeCode,
            string entityTypeCode,
            Country country)
        {
            // Arrange
            AddressFormQuery query = this.fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);
            EnumTypeItem enumTypeItem = this.fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            // Act + Assert
            Assert.Throws<DomainValidationException>(() => this.handler.Handle(query)).Errors.Should().Contain(e => e.PropertyName == nameof(query.CountryCode));
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void Given_InvalidEnumTypeItemInQuery_When_HandlingQuery_Then_ShouldThrowException(
            string enumTypeCode,
            string entityTypeCode,
            Country country)
        {
            // Arrange
            AddressFormQuery query = this.fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);

            this.mockedCountryData.Add(country);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            // Act + Assert
            Assert.Throws<DomainValidationException>(() => this.handler.Handle(query)).Errors.Should().Contain(e => e.PropertyName == nameof(query.EntityType));
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void Given_EnumTypeItemExistsButForInvalidEnumTypeInQuery_When_HandlingQuery_Then_ShouldThrowException(
            string enumTypeCode,
            string entityTypeCode,
            string otherEnumTypeCode,
            Country country)
        {
            // Arrange
            AddressFormQuery query = this.fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);
            EnumTypeItem enumTypeItem = this.fixture.BuildEnumTypeItem(otherEnumTypeCode, entityTypeCode);

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            // Act + Assert
            Assert.Throws<DomainValidationException>(() => this.handler.Handle(query)).Errors.Should().Contain(e => e.PropertyName == nameof(query.EntityType));
        }

        [Theory]
        [InlineAutoMoqData("EntityType")]
        public void
            Given_ExistingAddressForCountryAndEntityTypeInQuery_When_HandlingQuery_Then_ShouldReturnAddressForCountryAndEntityType(
            string enumTypeCode,
            string entityTypeCode,
            Country country,
            Country otherCountry,
            EnumTypeItem otherEnumTypeItem)
        {
            // Arrange
            country.Id = Guid.NewGuid();
            otherCountry.Id = Guid.NewGuid();
            AddressFormQuery query = this.fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);
            EnumTypeItem enumTypeItem = this.fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            AddressForm addressFormForOtherCountry = this.fixture.BuildAddressForm(enumTypeItem, otherCountry);
            AddressForm addressFormForOtherEntityType = this.fixture.BuildAddressForm(otherEnumTypeItem, country);
            AddressForm addressFormForEntityType = this.fixture.BuildAddressForm(enumTypeItem, country);

            this.mockedAddressFormData.AddRange(
                new[] { addressFormForEntityType, addressFormForOtherCountry, addressFormForOtherEntityType });
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
            EnumTypeItem otherEnumTypeItem)
        {
            // Arrange
            country.Id = Guid.NewGuid();
            otherCountry.Id = Guid.NewGuid();
            otherEnumTypeItem.EnumType = this.fixture.Create<EnumType>();
            AddressFormQuery query = this.fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);
            EnumTypeItem enumTypeItem = this.fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            AddressForm addressFormForOtherCountry = this.fixture.BuildAddressForm(enumTypeItem, otherCountry);
            AddressForm addressFormForOtherEntityType = this.fixture.BuildAddressForm(otherEnumTypeItem, country);
            AddressForm defaultAddressForm = this.fixture.BuildAddressForm(null, country);

            this.mockedAddressFormData.AddRange(
                new[] { defaultAddressForm, addressFormForOtherCountry, addressFormForOtherEntityType });
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
            EnumTypeItem otherEnumTypeItem)
        {
            // Arrange
            country.Id = Guid.NewGuid();
            otherCountry.Id = Guid.NewGuid();
            AddressFormQuery query = this.fixture.BuildAddressFormQuery(entityTypeCode, country.IsoCode);
            EnumTypeItem enumTypeItem = this.fixture.BuildEnumTypeItem(enumTypeCode, entityTypeCode);

            this.mockedCountryData.Add(country);
            this.mockedEnumTypeItemData.Add(enumTypeItem);
            this.countryRepository.Setup(x => x.Get()).Returns(this.mockedCountryData.AsQueryable());
            this.enumTypeItemRepository.Setup(x => x.Get()).Returns(this.mockedEnumTypeItemData.AsQueryable());

            AddressForm addressFormForOtherCountry = this.fixture.BuildAddressForm(enumTypeItem, otherCountry);
            AddressForm addressFormForOtherEntityType = this.fixture.BuildAddressForm(otherEnumTypeItem, country);

            this.mockedAddressFormData.AddRange(new[] { addressFormForOtherCountry, addressFormForOtherEntityType });
            this.addressFormRepository.Setup(x => x.Get()).Returns(this.mockedAddressFormData.AsQueryable());

            // Act
            AddressFormQueryResult queryResult = this.handler.Handle(query);

            // Assert
            queryResult.Should().BeNull();
        }
    }
}
