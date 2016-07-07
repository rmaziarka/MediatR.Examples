namespace KnightFrank.Antares.Domain.UnitTests.AreaBreakdown.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AreaBreakdown.Queries;
    using KnightFrank.Antares.Domain.AreaBreakdown.QueryHandlers;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;
    public class AreaBreakdownQueryHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_NotExistingProperty_When_Handling_Then_ShouldReturnEmptyResult(
            [Frozen] Mock<IReadGenericRepository<PropertyAreaBreakdown>> areaBreakdownRepository,
            List<PropertyAreaBreakdown> areaBreakdown,
            AreaBreakdownQuery query,
            AreaBreakdownQueryHandler handler)
        {
            areaBreakdownRepository.Setup(x => x.Get()).Returns(areaBreakdown.AsQueryable());

            IList<PropertyAreaBreakdown> result = handler.Handle(query);

            result.Should().BeEmpty();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingPropertyAndNullListOfAreas_When_Handling_Then_ShouldReturnResultLimitedToGivenPropertyId(
            [Frozen] Mock<IReadGenericRepository<PropertyAreaBreakdown>> areaBreakdownRepository,
            List<PropertyAreaBreakdown> areaBreakdownList,
            List<PropertyAreaBreakdown> expectedAreaBreakdown,
            AreaBreakdownQueryHandler handler,
            Fixture fixture)
        {
            AreaBreakdownQuery query = fixture.Build<AreaBreakdownQuery>().With(x => x.AreaIds, null).Create();

            expectedAreaBreakdown.ForEach(x => x.PropertyId = query.PropertyId);
            areaBreakdownList.AddRange(expectedAreaBreakdown);
            expectedAreaBreakdown = expectedAreaBreakdown.OrderBy(x => x.Order).ToList();

            areaBreakdownRepository.Setup(x => x.Get()).Returns(areaBreakdownList.AsQueryable());

            IList<PropertyAreaBreakdown> result = handler.Handle(query);

            result.Should().Equal(expectedAreaBreakdown, (ab1, ab2) => ab1.Id == ab2.Id);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingPropertyAndListOfAreas_When_Handling_Then_ShouldReturnResultLimitedToGivenAreaIdAndPropertyId(
            [Frozen] Mock<IReadGenericRepository<PropertyAreaBreakdown>> areaBreakdownRepository,
            List<PropertyAreaBreakdown> areaBreakdownList,
            List<PropertyAreaBreakdown> areaBreakdownListWithExpectedItem,
            AreaBreakdownQueryHandler handler,
            Fixture fixture)
        {
            AreaBreakdownQuery query = fixture.Build<AreaBreakdownQuery>().With(x => x.AreaIds, new[] { areaBreakdownListWithExpectedItem.First().Id }).Create();

            areaBreakdownListWithExpectedItem.ForEach(x => x.PropertyId = query.PropertyId);
            areaBreakdownList.AddRange(areaBreakdownListWithExpectedItem);

            areaBreakdownRepository.Setup(x => x.Get()).Returns(areaBreakdownList.AsQueryable());

            IList<PropertyAreaBreakdown> result = handler.Handle(query);

            result.Should().HaveCount(1);
            result.First().ShouldBeEquivalentTo(areaBreakdownListWithExpectedItem.First());
        }
    }
}