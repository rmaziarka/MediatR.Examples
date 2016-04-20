namespace KnightFrank.Antares.Domain.UnitTests.Characteristic.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Characteristic.Queries;
    using KnightFrank.Antares.Domain.Characteristic.QueryHandlers;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Characteristic")]
    [Collection("CharacteristicGroupsQueryHandler")]
    public class CharacteristicGroupsQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ResourceLocalisedQueryValid_When_Handling_Then_ShouldReturnDictionary(
            [Frozen] Mock<IReadGenericRepository<CharacteristicGroupUsage>> characteristicGroupUsageRepository,
            CharacteristicGroupsQueryHandler handler,
            CharacteristicGroupsQuery query,
            IFixture fixture)
        {
            // Arrange
            CharacteristicGroupUsage expected = this.CreateCharacteristicGroupUsage(fixture, query.PropertyTypeId, query.CountryCode);

            IList<CharacteristicGroupUsage> mockDataList = new List<CharacteristicGroupUsage> { 
                 expected,           
                 this.CreateCharacteristicGroupUsage(fixture, Guid.NewGuid(), query.CountryCode),
                 this.CreateCharacteristicGroupUsage(fixture, query.PropertyTypeId, query.CountryCode + "Other"), 
                 this.CreateCharacteristicGroupUsage(fixture, Guid.NewGuid(), query.CountryCode + "Other")
             };

            characteristicGroupUsageRepository.Setup(
                r => r.GetWithInclude(u => u.CharacteristicGroupItems.Select(x => x.Characteristic)))
                                              .Returns(mockDataList.AsQueryable());

            // Act
            IList<CharacteristicGroupUsage> characteristicGroupUsages = handler.Handle(query).ToList();

            // Asserts
            characteristicGroupUsages.Should().NotBeNull();
            characteristicGroupUsages.Should().HaveCount(1);
            characteristicGroupUsages.Should().Contain(expected);
            characteristicGroupUsageRepository.Verify(
                r => r.GetWithInclude(It.IsAny<Expression<Func<CharacteristicGroupUsage, object>>>()),
                Times.Once);
        }

        private CharacteristicGroupUsage CreateCharacteristicGroupUsage(IFixture fixture, Guid propertyTypeId, string countryCode)
        {
            Country country = fixture.Build<Country>().With(c => c.IsoCode, countryCode).Create();

            return
                fixture.Build<CharacteristicGroupUsage>()
                       .With(u => u.PropertyTypeId, propertyTypeId)
                       .With(u => u.CountryId, country.Id)
                       .With(u => u.Country, country)
                       .Create();
        }
    }
}
