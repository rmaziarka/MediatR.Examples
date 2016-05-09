namespace KnightFrank.Antares.Domain.UnitTests.Characteristic.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
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
            CharacteristicGroupUsage expected = this.CreateCharacteristicGroupUsage(fixture, query.PropertyTypeId, query.CountryId);

            IList<CharacteristicGroupUsage> mockDataList = new List<CharacteristicGroupUsage> {
                 expected,
                 this.CreateCharacteristicGroupUsage(fixture, Guid.NewGuid(), query.CountryId),
                 this.CreateCharacteristicGroupUsage(fixture, query.PropertyTypeId, Guid.NewGuid()),
                 this.CreateCharacteristicGroupUsage(fixture, Guid.NewGuid(), Guid.NewGuid())
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

        private CharacteristicGroupUsage CreateCharacteristicGroupUsage(IFixture fixture, Guid propertyTypeId, Guid countryId)
        {
            return
                fixture.Build<CharacteristicGroupUsage>()
                       .With(u => u.PropertyTypeId, propertyTypeId)
                       .With(u => u.CountryId, countryId)
                       .Create();
        }
    }
}
